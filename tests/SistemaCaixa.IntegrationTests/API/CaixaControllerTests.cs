using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SistemaCaixa.API;
using SistemaCaixa.Application.UseCases.AbrirCaixa;
using Xunit;

namespace SistemaCaixa.IntegrationTests.API;

public class CaixaControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CaixaControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task POST_AbrirCaixa_DeveRetornar200()
    {
        // Arrange
        var command = new AbrirCaixaCommand(100, "operador1");

        // Act
        var response = await _client.PostAsJsonAsync("/api/caixa/abrir", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<dynamic>();
        ((decimal)dto.saldoInicial).Should().Be(100);
    }
}
