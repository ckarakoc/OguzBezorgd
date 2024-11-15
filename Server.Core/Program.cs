using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Server.Core.Data;
using Server.Core.Entities;
using Server.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
);
var services = builder.Services;
var config = builder.Configuration;

// Configurations
services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

services.AddApplicationService(config);
services.AddIdentityService(config);

services.AddEndpointsApiExplorer(); // Enables API explorer
// services.AddSwaggerGen(); // Registers Swagger generator
services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();
var appLifetime = app.Lifetime;

// Middleware
app.UseCors("AllowLocalhost");

// Mapping
app.MapControllers();
app.MapIdentityApi<User>();

// Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Insert Roles
// using var scope = app.Services.CreateScope();
// var serviceProvider = scope.ServiceProvider;

// var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
// await context.Database.MigrateAsync();

// var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
// var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
// string[] roleNames = ["Customer", "Partner", "Deliverer", "Superman"];
// foreach (var roleName in roleNames)
// {
//     var roleExist = await roleManager.RoleExistsAsync(roleName);
//     if (!roleExist)
//     {
//         await roleManager.CreateAsync(new IdentityRole<int>(roleName));
//     }
// }

if (app.Environment.IsDevelopment())
{
    // Create a superuser who will maintain the web app only in development environment
    // var superuser = new User
    // {
    //     UserName = "superuser",
    // };

    // var userPwd = config["SuperUserPassword"];
    // var user = await userManager.FindByNameAsync(superuser.UserName);

    // if (user == null && userPwd != null)
    // {
    //     var createPowerUser = await userManager.CreateAsync(superuser, userPwd);
    //     if (createPowerUser.Succeeded)
    //     {
    //         await userManager.AddToRoleAsync(superuser, "Superman");
    //     }
    //     else
    //     {
    //         Console.WriteLine("Failed to create superuser");
    //     }
    // }

    // Seed.SeedData(userManager, roleManager, context, config);
    // appLifetime.ApplicationStopping.Register(() => { context.Database.EnsureDeleted(); });
}

app.MapGet("/", () => "Hello World!")
    .WithName("Home")
    .RequireAuthorization();

app.Run();
