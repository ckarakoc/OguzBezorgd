﻿using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Server.Core.Data;
using Server.Core.Data.Repositories;
using Server.Core.Entities;
using Server.Core.Interfaces;

namespace Server.Core.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        services.AddDbContext<ApplicationDbContext>(opt => { opt.UseSqlite(config.GetConnectionString("DefaultConnection")); });
        services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost", builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:4200", "https://localhost:4200");
            });
        });


        // Add repositories
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Swagger
        // services.AddEndpointsApiExplorer();
        // services.AddSwaggerGen();

        return services;
    }
}