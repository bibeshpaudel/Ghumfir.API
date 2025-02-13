using System.ComponentModel.DataAnnotations;
using Ghumfir.Domain.Constants.Enums;

namespace Ghumfir.Domain.Entities;

using System;

public class VerificationCode
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public MessageDeliveryChannelEnum Type { get; set; }
    public MessageDeliveryPurposeEnum Purpose { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsUsed { get; set; } 
    public DateTime CreatedAt { get; set; }
}
