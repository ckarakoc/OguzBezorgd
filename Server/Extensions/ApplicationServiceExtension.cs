using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Repositories;
using Server.Interfaces;

namespace Server.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers()
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        ;
        services.AddDbContext<DataContext>(opt => { opt.UseSqlite(config.GetConnectionString("DefaultConnection")); });
        services.AddCors();
        // Add repositories
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}