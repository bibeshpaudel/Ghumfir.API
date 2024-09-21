using Ghumfir.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using FluentValidation;
using Ghumfir.API.Models.AppSettingsModel;
using Ghumfir.Application.Contracts;
using Ghumfir.Application.DTOs.UserDTO;
using Ghumfir.Infrastructure.Accessors;
using Ghumfir.Infrastructure.Repositary.UserRepositary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ghumfir.Application.Validators.UserDTO;

namespace Ghumfir.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextService(configuration);
        
        var tokenSettings = configuration.GetSection("TokenSettings").Get<TokenSettingModel>();
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettingModel>();

        services.AddSingleton(tokenSettings);
        services.AddSingleton(jwtSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings!.Audience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Key))
            };
        });

        services.AddServiceConfiguration();

        return services;
    }

    private static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("GhumfirDb");
        services.AddDbContext<GhumfirDbContext>(
            options => options.UseNpgsql(connectionString,
                b => b.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.FullName)),
            ServiceLifetime.Scoped);
        
        return services;
    }
    
    private static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IValidator<RegisterUserDto>, RegisterDtoValidator>();
        services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
        services.AddScoped<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();
        services.AddScoped<IValidator<RefreshTokenDto>, RefreshTokenDtoValidator>();
        services.AddScoped<IValidator<ForgotPasswordDto>, ForgotPasswordDtoValidator>();
        services.AddScoped<IValidator<VerifyForgotPasswordDto>, VerifyForgotPasswordDtoValidator>();

        services.AddHttpContextAccessor();
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IUser, UserRepositary>();
        
        return services;
    }
}