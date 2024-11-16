using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Server.Core.Data;
using Server.Core.Entities;

namespace Server.Core.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
    {
        var secretKey = config["JwtSettings:SecretKey"]; // tokenKey
        
        services.AddIdentityCore<User>()
            .AddRoles<IdentityRole<int>>()
            .AddRoleManager<RoleManager<IdentityRole<int>>>()
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<User>>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            // options.Lockout.MaxFailedAccessAttempts = 50;
            // options.Lockout.AllowedForNewUsers = true;

            options.User.AllowedUserNameCharacters = config["User:AllowedUserNameCharacters"] ?? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._+";
            options.User.RequireUniqueEmail = false;
        });
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                    // options.Authority = null; 
                    // options.Audience = null;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ValidateIssuer = true, 
                        // ValidateAudience = true, 
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        // ValidIssuer = null,
                        // ValidAudience = null,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                }
            );
        services.AddAuthorization();
        return services;
    }
}