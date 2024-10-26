using Bogus;
using Microsoft.AspNetCore.Identity;
using Server.Entities;

namespace Server.Data;

public class Seed
{
    static Seed()
    {
        Console.WriteLine("Generating Random Seed...");
        Randomizer.Seed = new Random(123456);
    }


    public static async void SeedData(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager,
        IConfiguration config)
    {
        Console.WriteLine("Generating Users");
        var users = new Faker<User>()
            .RuleFor(u => u.UserName, f => f.Person.UserName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.EmailConfirmed, f => true)
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.PhoneNumberConfirmed, f => true)
            .RuleFor(u => u.TwoFactorEnabled, f => false)
            .RuleFor(u => u.LockoutEnabled, f => false)
            .RuleFor(u => u.LockoutEnd, f => null)
            .Generate(10);

        foreach (var user in users)
        {
            await userManager.CreateAsync(user, config["SuperUserPassword"] ?? "Pa$$w0rd");
        }
    }

    public static async void CleanupData(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager,
        IConfiguration config)
    {
        Console.WriteLine("Cleaning up Users...");
        var users = new Faker<User>()
            .RuleFor(u => u.UserName, f => f.Person.UserName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.EmailConfirmed, f => true)
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.PhoneNumberConfirmed, f => true)
            .RuleFor(u => u.TwoFactorEnabled, f => false)
            .RuleFor(u => u.LockoutEnabled, f => false)
            .RuleFor(u => u.LockoutEnd, f => null)
            .Generate(10);

        foreach (var user in userManager.Users)
        {
            await userManager.DeleteAsync(user);
        }
    }
}