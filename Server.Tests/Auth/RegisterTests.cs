using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Core.Data;
using Server.Tests.Factories;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Server.Tests.Auth;

public class RegisterTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;
    private const string UserName = "bogus";
    private const string Password = "Pa$$w0rd";

    public RegisterTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Register_Succeeds()
    {
        // Arrange
        var content = CreateRegisterContent(UserName, Password);
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Act
        _output.WriteLine(content.ReadAsStringAsync().Result);
        var httpResponse = await SendRegisterRequest(content);

        // Assert
        var users = await context.Users.ToListAsync();
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        users.Should().NotBeEmpty();
        users.Should().HaveCount(1);
    }


    [Theory]
    [RegisterFailsData]
    public async Task Register_Fails(string userName, string password, string errorMessage)
    {
        // Arrange
        var content = CreateRegisterContent(userName, password);

        // Act
        var httpResponse = await SendRegisterRequest(content);

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, errorMessage);
    }

    private JsonContent CreateRegisterContent(string userName, string password)
        => JsonContent.Create(new {userName, password});

    private async Task<HttpResponseMessage> SendRegisterRequest(JsonContent content)
        => await _client.PostAsync("/api/auth/register", content);

    private class RegisterFailsData : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // Username Tests
            yield return ["", Password, "UserName is required"];

            // Password Tests
            yield return [UserName, "", "Password is required"];
            yield return [UserName, Password[..7], "Password must be at least 8 characters"];
        }
    }
}

// todo
// [Fact]
// public async Task Register_ReturnsSuccessResult()
// {
//     // Arrange
//     var user = new User() { Email = "[email protected]", Password = "testpassword" };
//     var payload = JsonConvert.SerializeObject(user);
//     var content = new StringContent(payload, Encoding.UTF8, "application/json");
//
//     // Act
//     var response = await _client.PostAsync("/api/register", content);
//
//     // Assert
//     response.EnsureSuccessStatusCode();
//     var responseAsString = await response.Content.ReadAsStringAsync();
//     Assert.Contains("Success", responseAsString);
// }