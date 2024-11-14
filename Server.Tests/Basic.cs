using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Core.Data;
using Server.Core.Tests.Factories;
using Xunit.Abstractions;

namespace Server.Core.Tests;

public class Basic : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;
    private const string Username = "superuser";
    private const string Password = "Pa$$w0rd";

    public Basic(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
        _client = _factory.CreateClient();
        _factory.SeedDatabaseAsync().GetAwaiter().GetResult();
        _output.WriteLine("TEST");
    }

    [Fact]
    public async Task TestDatabaseContainsSeedData()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        var entities = await dbContext.Users.ToListAsync();
        _output.WriteLine(JsonSerializer.Serialize(entities));
        Assert.NotEmpty(entities); // Example assertion that data was seeded
    }

    [Fact]
    public async Task Ping()
    {
        var httpResponse = await _client.GetAsync("/");
        httpResponse.EnsureSuccessStatusCode().Should().BeSuccessful();
    }
}