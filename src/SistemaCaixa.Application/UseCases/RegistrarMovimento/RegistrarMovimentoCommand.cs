using MediatR;
using SistemaCaixa.Application;
using SistemaCaixa.Domain.ValueObjects;

namespace SistemaCaixa.Application.UseCases.RegistrarMovimento;

public record RegistrarMovimentoCommand(
    Guid CaixaId,
    TipoMovimento Tipo,
    Dinheiro Valor,
    string Descricao,
    string? NumeroDocumento,
    DateTime? Data
) : IRequest<Result<bool>>;
