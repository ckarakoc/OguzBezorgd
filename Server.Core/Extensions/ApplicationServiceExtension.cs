using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Server.Core.Data;
using Server.Core.Data.Repositories;
using Server.Core.Interfaces;
using Server.Core.Services;

namespace Server.Core.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers()
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        services.AddDbContext<DataContext>(opt => { opt.UseSqlite(config.GetConnectionString("DefaultConnection")); });
        services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost", builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:4200", "https://localhost:4200", "http://localhost:5152", "https://localhost:5152");
            });
        });

        // Add services
        services
            .AddScoped<ITokenService, TokenService>();

        // Add repositories
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            // {
            //     Name = "Authorization",
            //     Type = SecuritySchemeType.ApiKey,
            //     Scheme = "oauth2",
            //     In = ParameterLocation.Header,
            // });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer ' followed by your token."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

            options.IncludeXmlComments(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}