using SistemaCaixa.Domain.Entities;
using SistemaCaixa.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace SistemaCaixa.UnitTests.Domain;

public class CaixaTests
{
    [Fact]
    public void Abrir_DeveCriarCaixaAberto_ComSaldoInicialPositivo()
    {
        // Arrange
        var saldoInicial = Dinheiro.FromBRL(100);
        var operador = "operador1";

        // Act
        var caixa = Caixa.Abrir(saldoInicial, operador);

        // Assert
        caixa.Status.Should().Be(StatusCaixa.Aberto);
        caixa.SaldoInicial.Valor.Should().Be(100);
        caixa.OperadorAbertura.Should().Be(operador);
    }

    [Fact]
    public void Abrir_ComSaldoNegativo_DeveLancarExcecao()
    {
        // Arrange
        var saldoInicial = Dinheiro.FromBRL(-10);
        var operador = "operador1";

        // Act
        var acao = () => Caixa.Abrir(saldoInicial, operador);

        // Assert
        acao.Should().Throw<DomainException>().WithMessage("Saldo inicial não pode ser negativo");
    }
}
