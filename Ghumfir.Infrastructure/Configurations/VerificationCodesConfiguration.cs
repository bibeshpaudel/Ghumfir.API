using Ghumfir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ghumfir.Infrastructure.Configurations;

public class VerificationCodesConfiguration : IEntityTypeConfiguration<VerificationCode> 
{
    public void Configure(EntityTypeBuilder<VerificationCode> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Code) 
            .IsRequired()
            .HasMaxLength(6); 
        builder.Property(e => e.Type)
            .IsRequired();
        builder.Property(e => e.Purpose)
            .IsRequired();
        builder.Property(e => e.UserId)
            .IsRequired();
        builder.Property(e => e.ExpirationDate)
            .IsRequired();
        builder.Property(e => e.IsUsed)
            .IsRequired();
        builder.Property(e => e.CreatedAt)
            .IsRequired();
    }
}
