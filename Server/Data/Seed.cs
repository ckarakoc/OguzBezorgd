using Bogus;
using Microsoft.AspNetCore.Identity;
using Server.Entities;

namespace Server.Data;

public static class Seed
{
    private static List<User> _users = new Faker<User>()
        .RuleFor(u => u.UserName, f => f.Person.UserName)
        .RuleFor(u => u.Email, f => f.Person.Email)
        .RuleFor(u => u.EmailConfirmed, f => true)
        .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
        .RuleFor(u => u.PhoneNumberConfirmed, f => true)
        .RuleFor(u => u.TwoFactorEnabled, f => false)
        .RuleFor(u => u.LockoutEnabled, f => false)
        .RuleFor(u => u.LockoutEnd, f => null)
        .Generate(10);


    static Seed()
    {
        Console.WriteLine("Generating Random Seed...");
        Randomizer.Seed = new Random(123456);
    }


    /// <summary>
    /// Seed the database with all statically created data. 
    /// </summary>
    /// <param name="userManager"></param> serviceProvider.GetRequiredService&lt;UserManager&lt;User&gt;&gt;();
    /// <param name="roleManager"></param> serviceProvider.GetRequiredService&lt;RoleManager&lt;IdentityRole&lt;int&gt;&gt;&gt;
    /// <param name="config"></param> WebApplication.CreateBuilder(args).Configuration
    public static async void SeedData(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager,
        IConfiguration config)
    {
        Console.WriteLine("Generating Users");

        foreach (var user in _users)
        {
            await userManager.CreateAsync(user, config["SuperUserPassword"] ?? "Pa$$w0rd");
        }
    }

    /// <summary>
    /// Clean up all statically created variables.
    /// </summary>
    /// <param name="userManager"></param> serviceProvider.GetRequiredService&lt;UserManager&lt;User&gt;&gt;();
    /// <param name="roleManager"></param> serviceProvider.GetRequiredService&lt;RoleManager&lt;IdentityRole&lt;int&gt;&gt;&gt;
    /// <param name="config"></param> WebApplication.CreateBuilder(args).Configuration
    public static async void CleanupData(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager,
        IConfiguration config)
    {
        Console.WriteLine("Cleaning up Users...");

        foreach (var user in _users)
        {
            await userManager.DeleteAsync(user);
        }
    }
}