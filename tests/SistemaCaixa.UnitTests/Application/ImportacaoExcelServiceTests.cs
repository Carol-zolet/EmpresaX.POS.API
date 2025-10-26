using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SistemaCaixa.Application.DTOs;
using SistemaCaixa.Application.Services;
using Xunit;

namespace SistemaCaixa.UnitTests.Application;

public class ImportacaoExcelServiceTests
{
    [Theory]
    [InlineData("R$ 1.200,00", 1200.00)]
    [InlineData("R$47,00", 47.00)]
    [InlineData("R$ 9.765,35", 9765.35)]
    [InlineData("", null)]
    public void ParseValorBRL_ComDiferentesFormatos_DeveRetornarValorCorreto(string texto, decimal? esperado)
    {
        // Arrange
        var service = new ImportacaoExcelService(null, null);
        // Act
        var resultado = service.ParseValorBRL(texto);
        // Assert
        resultado.Should().Be(esperado);
    }
}
