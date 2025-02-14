using Ghumfir.API.Models.AppSettingsModel;
using Ghumfir.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ghumfir.Infrastructure.Repositary.UserRepositary
{
    public class TokenProvider(
        JwtSettingModel jwtSetting,
        TokenSettingModel tokenSetting
        )
    {
        public string GenerateAccessToken(ApplicationUser user, out DateTime expirationDate)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.Mobile),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("IsActive", user.IsActive.ToString()),
                new Claim("IsApproved", (!string.IsNullOrEmpty(user.ApprovedBy)).ToString()),
                new Claim("ForceChangePassword", user.ForceChangePassword.ToString())
            };

            expirationDate = DateTime.Now.AddMinutes(tokenSetting.AccessTokenExpirationInMinutes);

            var token = new JwtSecurityToken(
                jwtSetting.Issuer,
                jwtSetting.Audience,
                claims,
                expires: expirationDate,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(out DateTime expirationDate)
        {
            var randomNumber = new byte[64];
            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }
            expirationDate = DateTime.UtcNow.AddMinutes(tokenSetting.RefreshTokenExpirationInDays);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetTokenPrincipal(string token)
        {
            var validation = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSetting.Issuer,
                ValidAudience = jwtSetting.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key))
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }
    }
}
