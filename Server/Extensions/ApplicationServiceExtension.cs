using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt => { opt.UseSqlite(config.GetConnectionString("DefaultConnection")); });
        services.AddCors();
        
        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        return services;
    }
}