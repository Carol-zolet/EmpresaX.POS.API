using SistemaCaixa.Domain.ValueObjects;

namespace SistemaCaixa.Domain.Events;

public record CaixaAberto(Guid CaixaId, string Operador, Dinheiro SaldoInicial) : IDomainEvent;
