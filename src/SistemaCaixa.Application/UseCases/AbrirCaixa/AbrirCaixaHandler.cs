using MediatR;
using Microsoft.Extensions.Logging;
using SistemaCaixa.Application.DTOs;
using SistemaCaixa.Domain.Entities;
using SistemaCaixa.Domain.Repositories;

namespace SistemaCaixa.Application.UseCases.AbrirCaixa;

public class AbrirCaixaHandler : IRequestHandler<AbrirCaixaCommand, Result<CaixaDto>>
{
    private readonly ICaixaRepository _caixaRepository;
    private readonly ILogger<AbrirCaixaHandler> _logger;

    public AbrirCaixaHandler(ICaixaRepository caixaRepository, ILogger<AbrirCaixaHandler> logger)
    {
        _caixaRepository = caixaRepository;
        _logger = logger;
    }

    public async Task<Result<CaixaDto>> Handle(AbrirCaixaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var caixa = Caixa.Abrir(Dinheiro.FromBRL(request.SaldoInicial), request.OperadorAbertura);
            await _caixaRepository.AddAsync(caixa, cancellationToken);
            _logger.LogInformation("Caixa aberto: {CaixaId}", caixa.Id);
            var dto = new CaixaDto
            {
                Id = caixa.Id,
                DataAbertura = caixa.DataAbertura,
                SaldoInicial = caixa.SaldoInicial.Valor,
                SaldoFinal = caixa.SaldoFinal.Valor,
                Status = caixa.Status.ToString(),
                OperadorAbertura = caixa.OperadorAbertura,
                QuantidadeMovimentos = caixa.Movimentos.Count
            };
            return Result<CaixaDto>.Success(dto);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Erro de domínio ao abrir caixa");
            return Result<CaixaDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao abrir caixa");
            return Result<CaixaDto>.Failure("Erro ao abrir caixa");
        }
    }
}
