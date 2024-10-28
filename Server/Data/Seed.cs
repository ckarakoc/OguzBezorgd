using Bogus;
using Microsoft.AspNetCore.Identity;
using Server.Entities;

namespace Server.Data;

public static class Seed
{
    private static List<User> _customers = new Faker<User>()
        .RuleFor(u => u.UserName, f => f.Person.UserName)
        .RuleFor(u => u.Email, f => f.Person.Email)
        .RuleFor(u => u.EmailConfirmed, f => true)
        .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
        .RuleFor(u => u.PhoneNumberConfirmed, f => true)
        .RuleFor(u => u.TwoFactorEnabled, f => false)
        .RuleFor(u => u.LockoutEnabled, f => false)
        .RuleFor(u => u.LockoutEnd, f => null)
        .Generate(10);

    private static List<Store> _stores = new Faker<Store>()
        .RuleFor(s => s.StoreName, f => f.Company.CompanyName(2))
        .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber())
        .RuleFor(s => s.Email, f => f.Person.Email)
        .RuleFor(s => s.Website, f => f.Internet.Url())
        .RuleFor(s => s.OpeningTime, new TimeOnly(8, 0, 0))
        .RuleFor(s => s.ClosingTime, new TimeOnly(20, 0, 0))
        .RuleFor(s => s.Address, new Faker<StoreAddress>()
            .RuleFor(a => a.StreetAddress, f => f.Address.StreetAddress())
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.State, f => f.Address.State())
            .RuleFor(a => a.Country, f => f.Address.Country())
            .RuleFor(a => a.ZipCode, f => f.Address.ZipCode())
            .Generate()
        )
        .Generate(10);

    private static User _partner = new Faker<User>()
        .RuleFor(u => u.UserName, "partner")
        .RuleFor(u => u.Email, f => f.Person.Email)
        .RuleFor(u => u.EmailConfirmed, f => true)
        .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
        .RuleFor(u => u.PhoneNumberConfirmed, f => true)
        .RuleFor(u => u.TwoFactorEnabled, f => false)
        .RuleFor(u => u.LockoutEnabled, f => false)
        .RuleFor(u => u.LockoutEnd, f => null)
        .Generate();

    private static User _deliverer = new Faker<User>()
        .RuleFor(u => u.UserName, "deliverer")
        .RuleFor(u => u.Email, f => f.Person.Email)
        .RuleFor(u => u.EmailConfirmed, f => true)
        .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
        .RuleFor(u => u.PhoneNumberConfirmed, f => true)
        .RuleFor(u => u.TwoFactorEnabled, f => false)
        .RuleFor(u => u.LockoutEnabled, f => false)
        .RuleFor(u => u.LockoutEnd, f => null)
        .Generate();

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
        DataContext context, IConfiguration config)
    {
        Console.WriteLine("Generating Users");
        foreach (var user in _customers)
        {
            await userManager.CreateAsync(user, config["SuperUserPassword"] ?? "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Customer");
        }

        _partner.Stores.AddRange(_stores);
        foreach (var store in _stores)
        {
            Console.WriteLine(store.Address);
        }

        await userManager.CreateAsync(_partner, config["SuperUserPassword"] ?? "Pa$$w0rd");
        await userManager.AddToRoleAsync(_partner, "Partner");

        await userManager.CreateAsync(_deliverer, config["SuperUserPassword"] ?? "Pa$$w0rd");
        await userManager.AddToRoleAsync(_deliverer, "Deliverer");

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Clean up all statically created data.
    /// </summary>
    /// <param name="userManager"></param> serviceProvider.GetRequiredService&lt;UserManager&lt;User&gt;&gt;();
    /// <param name="roleManager"></param> serviceProvider.GetRequiredService&lt;RoleManager&lt;IdentityRole&lt;int&gt;&gt;&gt;
    /// <param name="config"></param> WebApplication.CreateBuilder(args).Configuration
    public static async void CleanupData(DataContext context)
    {
        Console.WriteLine("Dropping the Database...");
        await context.Database.EnsureDeletedAsync();
    }
}