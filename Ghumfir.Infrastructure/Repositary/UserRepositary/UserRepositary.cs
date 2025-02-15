using Ghumfir.Application.Contracts;
using Ghumfir.Application.Contracts.Authentication;
using Ghumfir.Application.DTOs;
using Ghumfir.Application.DTOs.UserDTO;
using Ghumfir.Domain.Entities;
using Ghumfir.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ghumfir.Infrastructure.Repositary.UserRepositary;

public class UserRepositary(GhumfirDbContext dbContext, IUserAccessor userAccessor, IPasswordHasher passwordHasher,ITokenProvider tokenProvider) : IUser
{
    public async Task<ApiResult<string?>> RegisterUser(RegisterUserDto request)
    {
        var getUser = await dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Mobile == request.Mobile);
        if (getUser != null)
        {
            return ApiResponse<string?>.Failed("User already exists", null);
        }

        string hashedPassword = passwordHasher.HashPassword(request.Password);

        var user = dbContext.ApplicationUsers.Add(new ApplicationUser()
        {
            Mobile = request.Mobile,
            Password = hashedPassword,
            Email = request.Email,
            FullName = request.FullName,
            Role = request.Role,
            IsActive = false,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow,
            ForceChangePassword = false
        });

        await dbContext.SaveChangesAsync();

        return ApiResponse<string?>.Success();
    }

    public async Task<ApiResult<LoginResponse>> LoginUser(LoginDto request)
    {
        var getUser = await dbContext.ApplicationUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Mobile == request.Mobile);
        if (getUser == null)
        {
            return ApiResponse<LoginResponse>.Failed("Invalid Mobile or Password");
        }

        bool checkPassword = passwordHasher.VerifyPassword(request.Password, getUser.Password);
        if (!checkPassword)
        {
            return ApiResponse<LoginResponse>.Failed("Invalid Mobile or Password");
        }

        var accessToken = tokenProvider.GenerateAccessToken(getUser, out var accessTokenExp);
        var refreshToken = tokenProvider.GenerateRefreshToken(out var refreshTokenExp);

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            return ApiResponse<LoginResponse>.Failed("Error while generating token");
        }

        var existingToken = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == getUser.Id);

        if (existingToken != null)
        {
            existingToken.Token = refreshToken;
            existingToken.ExpiresAt = refreshTokenExp;

            dbContext.RefreshTokens.Update(existingToken);
        }
        else
        {
            var newToken = new RefreshToken
            {
                Token = refreshToken,
                UserId = getUser.Id,
                ExpiresAt = refreshTokenExp
            };

            await dbContext.RefreshTokens.AddAsync(newToken);
        }

        await dbContext.SaveChangesAsync();

        return ApiResponse<LoginResponse>.Success(new LoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = accessTokenExp,
            ForceChangePassword = getUser.ForceChangePassword,
        });
    }

    public async Task<ApiResult<LoginResponse>> RefreshToken(RefreshTokenDto request)
    {
        var refreshToken = await dbContext.RefreshTokens
            .Where(rt => rt.Token == request.RefreshToken)
            .FirstOrDefaultAsync();

        if (refreshToken == null || refreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            return ApiResponse<LoginResponse>.Failed("Invalid Token");
        }

        var getUser = await dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == refreshToken.UserId);

        if (getUser == null)
        {
            return ApiResponse<LoginResponse>.Failed("Invalid Token");
        }

        var newJwtToken = tokenProvider.GenerateAccessToken(getUser, out var accessTokenExp);
        var newRefreshToken = tokenProvider.GenerateRefreshToken(out var refreshTokenExp);

        if (string.IsNullOrEmpty(newJwtToken) || string.IsNullOrEmpty(newRefreshToken))
        {
            return ApiResponse<LoginResponse>.Failed("Error while generating token");
        }

        refreshToken.Token = newRefreshToken;
        refreshToken.ExpiresAt = refreshTokenExp;

        dbContext.RefreshTokens.Update(refreshToken);

        await dbContext.SaveChangesAsync();

        return ApiResponse<LoginResponse>.Success(new LoginResponse()
        {
            AccessToken = newJwtToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = accessTokenExp
        });
    }

    public async Task<ApiResult<string?>> ChangePassword(ChangePasswordDto request)
    {
        var getUser = await dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Mobile == userAccessor.GetMobile());
        if (getUser == null)
        {
            return ApiResponse<string?>.Failed("Invalid Mobile or Password");
        }

        bool checkPassword = passwordHasher.VerifyPassword(request.OldPassword, getUser.Password);
        if (!checkPassword)
        {
            return ApiResponse<string?>.Failed("Invalid Mobile or Password");
        }

        string hashedNewPassword = passwordHasher.HashPassword(request.NewPassword);

        getUser.Password = hashedNewPassword;

        dbContext.ApplicationUsers.Update(getUser);

        await dbContext.SaveChangesAsync();

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
}