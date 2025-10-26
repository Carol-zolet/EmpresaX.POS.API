using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_ComCredenciaisValidas_DeveRetornarToken()
    {
        // Arrange
        var client = TestServerFactory.CreateClient();
        var payload = new { email = "admin@empresa.com", senha = "SenhaForte123" };

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", payload);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.True(json.TryGetProperty("token", out var tokenProp));
        Assert.False(string.IsNullOrWhiteSpace(tokenProp.GetString()));
    }
}
