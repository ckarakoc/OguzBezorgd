using Server.Core.Tests.Factories;
using Xunit.Abstractions;

namespace Server.Core.Tests.Auth;

public class RegisterTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;
    private const string Username = "superuser";
    private const string Password = "Pa$$w0rd";

    public RegisterTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
        _client = _factory.CreateClient();
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