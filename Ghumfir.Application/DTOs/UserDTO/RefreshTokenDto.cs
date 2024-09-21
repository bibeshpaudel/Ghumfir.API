using System.ComponentModel.DataAnnotations;

namespace Ghumfir.Application.DTOs.UserDTO;

public class RefreshTokenDto
{
    [Required]
    public string? AccessToken { get; set; } = string.Empty;
    [Required]
    public string? RefreshToken { get; set; } = string.Empty;
}