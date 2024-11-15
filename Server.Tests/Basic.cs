using System.Text.Json;
using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Core.Data;
using Server.Core.Entities;
using Server.Tests.Factories;
using Xunit.Abstractions;

namespace Server.Tests;

public class Basic : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public Basic(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task TestDatabaseContainsSeedData()
    {
        // Arrange
        var bogus = new Faker<User>()
            .RuleFor(u => u.UserName, "bogus")
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.EmailConfirmed, f => true)
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.PhoneNumberConfirmed, f => true)
            .RuleFor(u => u.TwoFactorEnabled, f => false)
            .RuleFor(u => u.LockoutEnabled, f => false)
            .RuleFor(u => u.LockoutEnd, f => null)
            .Generate();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.Users.AddAsync(bogus);
        await context.SaveChangesAsync();

        // Act
        var users = await context.Users.ToListAsync();

        // Assert
        var options = new JsonSerializerOptions {WriteIndented = true};
        _output.WriteLine(JsonSerializer.Serialize(users, options));
        users.Should().NotBeEmpty();
        users.Should().HaveCount(1);
    }

    [Fact]
    public async Task Ping()
    {
        var httpResponse = await _client.GetAsync("/");
        _output.WriteLine(httpResponse.Content.ReadAsStringAsync().Result);
        httpResponse.EnsureSuccessStatusCode().Should().BeSuccessful();
    }
}