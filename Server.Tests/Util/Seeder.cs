using Bogus;
using Server.Core.Data;
using Server.Core.Entities;

namespace Server.Core.Tests.Util;

public class Seeder
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
    
    public static async Task SeedAsync(DataContext context)
    {
        
    }
}