using MediatR;
using SistemaCaixa.Application.DTOs;

namespace SistemaCaixa.Application.UseCases.AbrirCaixa;

public record AbrirCaixaCommand(decimal SaldoInicial, string OperadorAbertura) : IRequest<Result<CaixaDto>>;
