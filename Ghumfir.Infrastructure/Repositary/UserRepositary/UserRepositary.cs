﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ghumfir.API.Models.AppSettingsModel;
using Ghumfir.Application.Contracts;
using Ghumfir.Application.DTOs;
using Ghumfir.Application.DTOs.UserDTO;
using Ghumfir.Application.Services;
using Ghumfir.Domain.Entities;
using Ghumfir.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Ghumfir.Infrastructure.Repositary.UserRepositary;

public class UserRepositary : IUser
{
    private readonly GhumfirDbContext _dbContext;
    private readonly JwtSettingModel _jwtSetting;
    private readonly TokenSettingModel _tokenSetting;
    private readonly IUserAccessor _userAccessor;

    public UserRepositary(GhumfirDbContext dbContext, JwtSettingModel jwtSetting, TokenSettingModel tokenSetting, IUserAccessor userAccessor)
    {
        _dbContext = dbContext;
        _jwtSetting = jwtSetting;
        _tokenSetting = tokenSetting;
        _userAccessor = userAccessor;
    }

    public async Task<ApiResult<string?>> RegisterUser(RegisterUserDto request)
    {
        var getUser = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Mobile == request.Mobile);
        if (getUser != null)
        {
            return ApiResponse<string?>.Failed("User already exists", null);
        }

        string hashedPassword = PasswordHasher.HashPassword(request.Password);

        var user = _dbContext.ApplicationUsers.Add(new ApplicationUser()
        {
            Mobile = request.Mobile,
            Password = hashedPassword,
            Email = request.Email,
            FullName = request.FullName,
            Role = request.Role,
            IsActive = false,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow,
        });

        await _dbContext.SaveChangesAsync();

        return ApiResponse<string?>.Success();
    }

    public async Task<ApiResult<LoginResponse>> LoginUser(LoginDto request)
    {
        var getUser = await _dbContext.ApplicationUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Mobile == request.Mobile);
        if (getUser == null)
        {
            return ApiResponse<LoginResponse>.Failed("Invalid Mobile or Password");
        }

        bool checkPassword = PasswordHasher.VerifyPassword(request.Password, getUser.Password);
        if (!checkPassword)
        {
            return ApiResponse<LoginResponse>.Failed("Invalid Mobile or Password");
        }

        var accessToken = GenerateJwtToken(getUser, out var accessTokenExp);
        var refreshToken = GenerateRefreshToken(out var refreshTokenExp);

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            return ApiResponse<LoginResponse>.Failed("Error while generating token");
        }

        var existingToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == getUser.Id);

        if (existingToken != null)
        {
            existingToken.Token = refreshToken;
            existingToken.ExpiresAt = refreshTokenExp;

            _dbContext.RefreshTokens.Update(existingToken);
        }
        else
        {
            var newToken = new RefreshToken
            {
                Token = refreshToken,
                UserId = getUser.Id,
                ExpiresAt = refreshTokenExp
            };

            await _dbContext.RefreshTokens.AddAsync(newToken);
        }

        await _dbContext.SaveChangesAsync();

        return ApiResponse<LoginResponse>.Success(new LoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = accessTokenExp
        });
    }

    public async Task<ApiResult<LoginResponse>> RefreshToken(RefreshTokenDto request)
    {
        var principal = GetTokenPrincipal(request.AccessToken);
        if (principal == null)
            return ApiResponse<LoginResponse>.Failed("Invalid Token");

        var userId = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return ApiResponse<LoginResponse>.Failed("User not found");

        var refreshToken = await _dbContext.RefreshTokens
            .Where(rt => rt.Token == request.RefreshToken && rt.UserId.ToString() == userId)
            .FirstOrDefaultAsync();

        var getUser = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        if (getUser is null || refreshToken == null || refreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            return ApiResponse<LoginResponse>.Failed("Invalid Token");
        }

        var newJwtToken = GenerateJwtToken(getUser, out var accessTokenExp);
        var newRefreshToken = GenerateRefreshToken(out var refreshTokenExp);

        if (string.IsNullOrEmpty(newJwtToken) || string.IsNullOrEmpty(newRefreshToken))
        {
            return ApiResponse<LoginResponse>.Failed("Error while generating token");
        }

        refreshToken.Token = newRefreshToken;
        refreshToken.ExpiresAt = refreshTokenExp;

        _dbContext.RefreshTokens.Update(refreshToken);

        await _dbContext.SaveChangesAsync();

        return ApiResponse<LoginResponse>.Success(new LoginResponse()
        {
            AccessToken = newJwtToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = accessTokenExp
        });
    }

    public async Task<ApiResult<string?>> ChangePassword(ChangePasswordDto request)
    {
        var getUser = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Mobile == _userAccessor.GetMobile());
        if (getUser == null)
        {
            return ApiResponse<string?>.Failed("Invalid Mobile or Password");
        }

        bool checkPassword = PasswordHasher.VerifyPassword(request.OldPassword, getUser.Password);
        if (!checkPassword)
        {
            return ApiResponse<string?>.Failed("Invalid Mobile or Password");
        }

        string hashedNewPassword = PasswordHasher.HashPassword(request.NewPassword);

        getUser.Password = hashedNewPassword;

        _dbContext.ApplicationUsers.Update(getUser);

        await _dbContext.SaveChangesAsync();

        return ApiResponse<string?>.Success();
    }

    public Task<ApiResult<ForgotPasswordResponse>> ForgotPassword(ForgotPasswordDto request)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResult<string?>> VerifyForgotPassword(VerifyForgotPasswordDto request)
    {
        throw new NotImplementedException();
    }

    private ClaimsPrincipal? GetTokenPrincipal(string token)
    {
        var validation = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = _jwtSetting.Issuer,
            ValidAudience = _jwtSetting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key))
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }

    private string? GenerateRefreshToken(out DateTime expirationDate)
    {
        var randomNumber = new byte[64];

        using (var numberGenerator = RandomNumberGenerator.Create())
        {
            numberGenerator.GetBytes(randomNumber);
        }

        expirationDate = DateTime.UtcNow.AddMinutes(_tokenSetting.RefreshTokenExpirationInDays);

        return Convert.ToBase64String(randomNumber);
    }

    private string? GenerateJwtToken(ApplicationUser user, out DateTime expirationDate)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.MobilePhone, user.Mobile),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("IsActive", user.IsActive.ToString()),
            new Claim("IsApproved", (!string.IsNullOrEmpty(user.ApprovedBy)).ToString())
        };

        expirationDate = DateTime.UtcNow.AddMinutes(_tokenSetting.AccessTokenExpirationInMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSetting.Issuer,
            audience: _jwtSetting.Audience,
            claims: userClaims,
            expires: expirationDate,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}