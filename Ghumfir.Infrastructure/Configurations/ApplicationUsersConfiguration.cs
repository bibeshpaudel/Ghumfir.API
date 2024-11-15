using Ghumfir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ghumfir.Infrastructure.Configurations;

public class ApplicationUsersConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Mobile)
            .IsRequired()
            .HasMaxLength(15);
        builder.Property(e => e.FullName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(e => e.Password)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(e => e.Email)
            .HasMaxLength(150);
        builder.Property(e => e.IsActive)
            .IsRequired();
        builder.Property(e => e.IsDeleted)
            .IsRequired();
        builder.Property(e => e.IsEmailVerified);
        builder.Property(e => e.Role)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(e => e.CreatedDate)
            .IsRequired();
        builder.Property(e => e.DeletedDate);
        builder.Property(e => e.ApprovedBy);
        builder.Property(e => e.ApprovedDate);
    }
}