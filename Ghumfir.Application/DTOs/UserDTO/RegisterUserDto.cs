using System.ComponentModel.DataAnnotations;

namespace Ghumfir.Application.DTOs.UserDTO;

public class RegisterUserDto
{
    public string? Mobile { get; set; } = string.Empty;
    public string? FullName { get; set; } = string.Empty;
    public string? Password { get; init; } = string.Empty;
    public string? ConfirmPassword { get; set; } = string.Empty;
    public string? Role { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
}