using System.ComponentModel.DataAnnotations;

namespace Ghumfir.Domain.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Token { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public DateTime ExpiresAt { get; set; }
}