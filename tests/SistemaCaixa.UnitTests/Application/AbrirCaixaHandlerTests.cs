using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SistemaCaixa.Application.DTOs;
using SistemaCaixa.Application.UseCases.AbrirCaixa;
using SistemaCaixa.Domain.Entities;
using SistemaCaixa.Domain.Repositories;
using Xunit;

namespace SistemaCaixa.UnitTests.Application;

public class AbrirCaixaHandlerTests
{
    [Fact]
    public async Task Handle_ComandoValido_DeveRetornarSucesso()
    {
        // Arrange
        var repoMock = new Mock<ICaixaRepository>();
        repoMock.Setup(r => r.AddAsync(It.IsAny<Caixa>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var loggerMock = new Mock<ILogger<AbrirCaixaHandler>>();
        var handler = new AbrirCaixaHandler(repoMock.Object, loggerMock.Object);
        var command = new AbrirCaixaCommand(100, "operador1");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.SaldoInicial.Should().Be(100);
        repoMock.Verify(r => r.AddAsync(It.IsAny<Caixa>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
