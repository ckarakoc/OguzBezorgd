using FluentAssertions;
using Server.Tests.Factories;
using Xunit.Abstractions;

namespace Server.Tests;

public class Basic : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;
    private const string Username = "superuser";
    private const string Password = "Pa$$w0rd";

    public Basic(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        factory.Dispose();
        _factory = factory;
        _output = output;
    }

    [Fact]
    public async Task Ping()
    {
        var _client = _factory.CreateClient();
        var httpResponse = await _client.GetAsync("/");
        httpResponse.EnsureSuccessStatusCode().Should().BeSuccessful();
    }
}