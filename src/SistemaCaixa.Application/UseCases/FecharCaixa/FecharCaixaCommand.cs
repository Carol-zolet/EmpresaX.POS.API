using MediatR;
using SistemaCaixa.Application.DTOs;

namespace SistemaCaixa.Application.UseCases.FecharCaixa;

public record FecharCaixaCommand(Guid CaixaId, string OperadorFechamento) : IRequest<Result<RelatorioFechamentoDto>>;
