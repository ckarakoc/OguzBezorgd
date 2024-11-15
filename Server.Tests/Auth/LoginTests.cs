using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Server.Tests.Factories;
using Xunit.Abstractions;

namespace Server.Tests.Auth;

public class LoginTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;
    private const string UserName = "bogus";
    private const string Password = "Pa$$w0rd";

    public LoginTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Login_Succeeds()
    {
        // Arrange
        var content = CreateRegisterContent(UserName, Password);
        await SendRegisterRequest(content);
        content = CreateLoginContent(UserName, Password);

        // Act
        var httpResponse = await SendLoginRequest(content);

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        httpResponse.EnsureSuccessStatusCode().Should().BeSuccessful();
    }

    [Fact]
    public async Task Login_Fails()
    {
        // Arrange
        var content = CreateRegisterContent(UserName, Password);
        await SendRegisterRequest(content);
        content = CreateLoginContent(UserName, Password + "1");

        // Act
        _output.WriteLine(content.ReadAsStringAsync().Result);
        var httpResponse = await SendLoginRequest(content);

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private JsonContent CreateRegisterContent(string userName, string password)
        => JsonContent.Create(new {userName, password});

    private JsonContent CreateLoginContent(string userName, string password)
        => JsonContent.Create(new {userName, password});

    private async Task<HttpResponseMessage> SendLoginRequest(JsonContent content)
        => await _client.PostAsync("/api/auth/login", content);

    private async Task<HttpResponseMessage> SendRegisterRequest(JsonContent content)
        => await _client.PostAsync("/api/auth/register", content);
}