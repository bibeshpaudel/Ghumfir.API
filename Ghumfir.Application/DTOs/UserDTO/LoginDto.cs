using System.ComponentModel.DataAnnotations;

namespace Ghumfir.Application.DTOs.UserDTO;

public class LoginDto
{
    public string? Mobile { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}