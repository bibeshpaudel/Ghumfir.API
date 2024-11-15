using System.ComponentModel.DataAnnotations;

namespace Ghumfir.Application.DTOs.UserDTO;

public class RefreshTokenDto
{
    public string? AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
}