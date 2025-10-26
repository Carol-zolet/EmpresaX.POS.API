using MediatR;
using Microsoft.Extensions.Logging;
using SistemaCaixa.Application.DTOs;
using SistemaCaixa.Domain.Entities;
using SistemaCaixa.Domain.Repositories;

namespace SistemaCaixa.Application.UseCases.FecharCaixa;

public class FecharCaixaHandler : IRequestHandler<FecharCaixaCommand, Result<RelatorioFechamentoDto>>
{
    private readonly ICaixaRepository _caixaRepository;
    private readonly ILogger<FecharCaixaHandler> _logger;

    public FecharCaixaHandler(ICaixaRepository caixaRepository, ILogger<FecharCaixaHandler> logger)
    {
        _caixaRepository = caixaRepository;
        _logger = logger;
    }

    public async Task<Result<RelatorioFechamentoDto>> Handle(FecharCaixaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var caixa = await _caixaRepository.GetByIdAsync(request.CaixaId, cancellationToken);
            if (caixa == null)
            {
                _logger.LogWarning("Caixa {CaixaId} não encontrado", request.CaixaId);
                return Result<RelatorioFechamentoDto>.NotFound("Caixa não encontrado");
            }
            var relatorio = caixa.Fechar(request.OperadorFechamento);
            await _caixaRepository.UpdateAsync(caixa, cancellationToken);
            _logger.LogInformation("Caixa {CaixaId} fechado com sucesso", request.CaixaId);
            var dto = new RelatorioFechamentoDto
            {
                CaixaId = relatorio.CaixaId,
                DataAbertura = relatorio.DataAbertura,
                DataFechamento = relatorio.DataFechamento,
                SaldoInicial = relatorio.SaldoInicial.Valor,
                TotalEntradas = relatorio.TotalEntradas.Valor,
                TotalSaidas = relatorio.TotalSaidas.Valor,
                SaldoCalculado = relatorio.SaldoCalculado.Valor,
                SaldoFinal = relatorio.SaldoFinal.Valor,
                Divergencia = relatorio.Divergencia.Valor,
                QuantidadeMovimentos = relatorio.QuantidadeMovimentos,
                TemDivergencia = relatorio.TemDivergencia
            };
            return Result<RelatorioFechamentoDto>.Success(dto);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Erro de domínio ao fechar caixa {CaixaId}", request.CaixaId);
            return Result<RelatorioFechamentoDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao fechar caixa {CaixaId}", request.CaixaId);
            return Result<RelatorioFechamentoDto>.Failure("Erro ao fechar caixa");
        }
    }
}
