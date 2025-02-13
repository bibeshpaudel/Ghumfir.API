using System.ComponentModel.DataAnnotations;

namespace Ghumfir.Application.DTOs.UserDTO;

public class LoginDto
{
    public string? Mobile { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string? AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool ForceChangePassword { get; set; }
}