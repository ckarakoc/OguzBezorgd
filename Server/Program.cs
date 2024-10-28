using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Entities;
using Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

// Configurations
services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

services.AddApplicationService(config);
services.AddIdentityService(config);

var app = builder.Build();
var appLifetime = app.Lifetime;


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Middleware

app.MapControllers();

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

// Create a superuser who will maintain the web app
var superuser = new User
{
    UserName = "superuser",
};

var userPWD = config["SuperUserPassword"];
var _user = await userManager.FindByNameAsync(superuser.UserName);

if (_user == null && userPWD != null)
{
    var createPowerUser = await userManager.CreateAsync(superuser, userPWD);
    if (createPowerUser.Succeeded)
    {
        await userManager.AddToRoleAsync(superuser, "Superman");
    }
    else
    {
        Console.WriteLine("Failed to create superuser");
    }
}

if (app.Environment.IsDevelopment())
{
    Seed.SeedData(userManager, roleManager, context, config);
    appLifetime.ApplicationStopping.Register(() => { Seed.CleanupData(context); });
}

app.MapGet("/", () => "Hello World!");

app.Run();