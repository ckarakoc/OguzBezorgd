using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Server.Core.Data;
using Server.Core.Entities;
using Server.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Host.UseSerilog((context, loggerConfig) => { loggerConfig.ReadFrom.Configuration(context.Configuration); }
);
var services = builder.Services;
var config = builder.Configuration;

// Configurations
services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

services.AddApplicationService(config);
services.AddIdentityService(config);

var app = builder.Build();
var appLifetime = app.Lifetime;

// Middleware
app.UseCors("AllowLocalhost");
app.UseAuthentication(); // before UseAuthorization() and MapControllers
app.UseAuthorization(); // before MapControllers()


app.UseDefaultFiles();
// app.UseStaticFiles();

app.UseHttpsRedirection();

// Mapping
app.MapControllers();

// Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt => { opt.ConfigObject.AdditionalItems.Add("persistAuthorization", "true"); });
}


// Insert Roles
using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;

var context = serviceProvider.GetRequiredService<DataContext>();
await context.Database.MigrateAsync();

var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
string[] roleNames = ["Customer", "Partner", "Deliverer", "Superman"];
foreach (var roleName in roleNames)
{
    var roleExist = await roleManager.RoleExistsAsync(roleName);
    if (!roleExist)
    {
        await roleManager.CreateAsync(new IdentityRole<int>(roleName));
    }
}

if (app.Environment.IsDevelopment())
{
    // Create a superuser who will maintain the web app only in development environment
    var superuser = new User
    {
        UserName = "superuser",
    };

    var userPwd = config["SuperUserPassword"];
    var user = await userManager.FindByNameAsync(superuser.UserName);

    if (user == null && userPwd != null)
    {
        var createPowerUser = await userManager.CreateAsync(superuser, userPwd);
        if (createPowerUser.Succeeded)
        {
            await userManager.AddToRoleAsync(superuser, "Superman");
        }
        else
        {
            Log.Warning("Failed to create superuser");
        }
    }

    // Seed.SeedData(userManager, roleManager, context, config);
    appLifetime.ApplicationStopping.Register(() =>
    {
        Log.CloseAndFlush();
        context.Database.EnsureDeleted();
    });
}

app.MapGet("/", () => "Hello World!");

app.Run();

public partial class Program
{
}