using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using SistemaCaixa.API;
using SistemaCaixa.Application.DTOs;
using Xunit;

namespace SistemaCaixa.IntegrationTests.API;

public class ImportacaoControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ImportacaoControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ImportarPlanilhaCaixa_ComArquivoValido_DeveCriarCaixaEMovimentos()
    {
        // Arrange
        var arquivo = CriarArquivoExcelTeste();
        var formFile = new StreamContent(arquivo);
        formFile.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        var form = new MultipartFormDataContent();
        form.Add(formFile, "arquivo", "controle_novembro.xlsx");

        // Act
        var response = await _client.PostAsync("/api/importacao/planilha-caixa", form);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var resultado = await response.Content.ReadFromJsonAsync<ImportacaoExcelResultDto>();
        resultado.Sucesso.Should().BeTrue();
        resultado.CaixaId.Should().NotBeNull();
        resultado.MovimentosImportados.Should().BeGreaterThan(0);
        resultado.ErrosEncontrados.Should().Be(0);
    }

    private Stream CriarArquivoExcelTeste()
    {
        // Gera um arquivo Excel em memória no formato esperado
        using var ms = new MemoryStream();
        using (var workbook = new ClosedXML.Excel.XLWorkbook())
        {
            var ws = workbook.Worksheets.Add("Controle Mensal");
            ws.Cell(1, 1).Value = "CONTROLE MENSAL NOVEMBRO";
            ws.Cell(4, 4).Value = 1000.00;
            ws.Cell(9, 1).Value = 1;
            ws.Cell(9, 2).Value = "Paciente Teste";
            ws.Cell(9, 3).Value = "Unimed";
            ws.Cell(9, 4).Value = 500.00;
            ws.Cell(10, 1).Value = 2;
            ws.Cell(10, 2).Value = "Consulta";
            ws.Cell(10, 3).Value = "Particular";
            ws.Cell(10, 5).Value = 200.00;
            workbook.SaveAs(ms);
        }
        ms.Position = 0;
        return new MemoryStream(ms.ToArray());
    }
}
