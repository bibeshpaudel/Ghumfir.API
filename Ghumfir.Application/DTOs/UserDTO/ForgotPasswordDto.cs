namespace Ghumfir.Application.DTOs.UserDTO;

public class ForgotPasswordDto
{
    public string? Mobile { get; set; } = string.Empty;
}

public class ForgotPasswordResponse
{
    public string? Mobile { get; set; } = string.Empty;
    public int Code { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class VerifyForgotPasswordDto
{
    public string? Mobile { get; set; } = string.Empty;
    public int Code { get; set; }
    public string? NewPassword { get; set; } = string.Empty;
}