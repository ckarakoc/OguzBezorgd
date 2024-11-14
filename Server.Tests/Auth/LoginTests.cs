using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using FluentAssertions;
using Server.Tests.Factories;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Server.Tests.Auth;

public class LoginTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;
    private const string Username = "superuser";
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
        var content = CreateLoginContent(Username, Password);
        var httpResponse = await SendLoginRequest(content);

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        httpResponse.EnsureSuccessStatusCode().Should().BeSuccessful();
    }

    [Theory]
    [LoginFailsData]
    public async Task Login_Fails(string username, string password, string errorMessage)
    {
        var content = CreateLoginContent(username, password);
        _output.WriteLine(content.ReadAsStringAsync().Result);
        var httpResponse = await SendLoginRequest(content);
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, errorMessage);
    }

    private JsonContent CreateLoginContent(string username, string password)
        => JsonContent.Create(new {username, password});

    private async Task<HttpResponseMessage> SendLoginRequest(JsonContent content)
        => await _client.PostAsync("/api/auth/login", content);

    class LoginFailsData : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return ["", Password, "Username is required"];
            yield return [Username, "", "Password is required"];
            yield return [Username, Password[..7], "Password must be at least 8 characters"];
            yield return [Username, Password + "1", "Wrong password"];
        }
    }
}