using Ghumfir.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Ghumfir.Infrastructure.Configurations;

namespace Ghumfir.Infrastructure.Persistence
{
    public class GhumfirDbContext : DbContext
    {
        public GhumfirDbContext(DbContextOptions<GhumfirDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<Enum>().HaveConversion<string>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationUsersConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokensConfiguration());
            modelBuilder.ApplyConfiguration(new VerificationCodesConfiguration());
        }
    }
}