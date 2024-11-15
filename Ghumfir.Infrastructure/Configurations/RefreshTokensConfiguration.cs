using Ghumfir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ghumfir.Infrastructure.Configurations;

public class RefreshTokensConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Token)
            .IsRequired();
        builder.Property(e => e.UserId)
            .IsRequired();
        builder.Property(e => e.ExpiresAt)
            .IsRequired();
    }
}
