using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SistemaCaixa.API;
using SistemaCaixa.Application.UseCases.AbrirCaixa;
using SistemaCaixa.Application.UseCases.FecharCaixa;
using Xunit;

namespace SistemaCaixa.E2ETests;

public class FluxoCaixaCompletoTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public FluxoCaixaCompletoTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AbrirEFecharCaixa_FluxoCompleto_DeveRetornarSucesso()
    {
        // Arrange
        var abrirCommand = new AbrirCaixaCommand(200, "operadorE2E");
        var responseAbrir = await _client.PostAsJsonAsync("/api/caixa/abrir", abrirCommand);
        responseAbrir.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await responseAbrir.Content.ReadFromJsonAsync<dynamic>();
        Guid caixaId = dto.id;

        // Act
        var fecharCommand = new FecharCaixaCommand(caixaId, "operadorE2E");
        var responseFechar = await _client.PostAsJsonAsync("/api/caixa/fechar", fecharCommand);

        // Assert
        responseFechar.StatusCode.Should().Be(HttpStatusCode.OK);
        var relatorio = await responseFechar.Content.ReadFromJsonAsync<dynamic>();
        ((decimal)relatorio.saldoFinal).Should().Be(200);
        ((bool)relatorio.temDivergencia).Should().BeFalse();
    }
}
