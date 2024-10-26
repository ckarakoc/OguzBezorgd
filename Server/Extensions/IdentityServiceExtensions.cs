using Microsoft.AspNetCore.Identity;
using Server.Data;
using Server.Entities;

namespace Server.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<User>()
            .AddRoles<IdentityRole<int>>()
            .AddRoleManager<RoleManager<IdentityRole<int>>>()
            .AddEntityFrameworkStores<DataContext>();
        
        services.AddAuthentication();
        return services;
    }
}