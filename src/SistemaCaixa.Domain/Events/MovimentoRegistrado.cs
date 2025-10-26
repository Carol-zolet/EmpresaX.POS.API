using SistemaCaixa.Domain.ValueObjects;

namespace SistemaCaixa.Domain.Events;

public record MovimentoRegistrado(Guid CaixaId, Guid MovimentoId, TipoMovimento Tipo, Dinheiro Valor) : IDomainEvent;
