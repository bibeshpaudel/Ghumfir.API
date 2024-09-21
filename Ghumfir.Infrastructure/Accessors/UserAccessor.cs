using System.Security.Claims;
using Ghumfir.Application.Contracts;
using Microsoft.AspNetCore.Http;

namespace Ghumfir.Infrastructure.Accessors;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

    public string GetUserId()
    {
        return User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public string GetFullname()
    {
        return User?.FindFirstValue(ClaimTypes.Name);
    }

    public string GetMobile()
    {
        return User?.FindFirstValue(ClaimTypes.MobilePhone);
    }
    
    public string GetRole()
    {
        return User?.FindFirstValue(ClaimTypes.Role);
    }
    
    public string GetActiveStatus()
    {
        return User?.FindFirstValue("IsActive");
    }
    
    public string GetApprovalStatus()
    {
        return User?.FindFirstValue("IsApproved");
    }
}