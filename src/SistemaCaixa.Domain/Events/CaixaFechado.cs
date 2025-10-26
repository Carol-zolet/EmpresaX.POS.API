using SistemaCaixa.Domain.ValueObjects;

namespace SistemaCaixa.Domain.Events;

public record CaixaFechado(Guid CaixaId, string Operador, Dinheiro SaldoFinal, RelatorioFechamento Relatorio) : IDomainEvent;
