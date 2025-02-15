using System.Security.Claims;
using Ghumfir.Domain.Entities;

namespace Ghumfir.Application.Contracts.Authentication;

public interface ITokenProvider
{
    string GenerateAccessToken(ApplicationUser user, out DateTime expirationDate);
    string GenerateRefreshToken(out DateTime expirationDate);
    ClaimsPrincipal? GetTokenPrincipal(string token);
}