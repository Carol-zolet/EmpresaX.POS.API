using MediatR;
using Microsoft.Extensions.Logging;
using SistemaCaixa.Application;
using SistemaCaixa.Domain.Entities;
using SistemaCaixa.Domain.Repositories;
using SistemaCaixa.Domain.ValueObjects;

namespace SistemaCaixa.Application.UseCases.RegistrarMovimento;

public class RegistrarMovimentoHandler : IRequestHandler<RegistrarMovimentoCommand, Result<bool>>
{
    private readonly ICaixaRepository _caixaRepository;
    private readonly ILogger<RegistrarMovimentoHandler> _logger;

    public RegistrarMovimentoHandler(ICaixaRepository caixaRepository, ILogger<RegistrarMovimentoHandler> logger)
    {
        _caixaRepository = caixaRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(RegistrarMovimentoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var caixa = await _caixaRepository.GetByIdAsync(request.CaixaId, cancellationToken);
            if (caixa == null)
            {
                _logger.LogWarning("Caixa {CaixaId} não encontrado", request.CaixaId);
                return Result<bool>.NotFound("Caixa não encontrado");
            }
            caixa.RegistrarMovimento(
                request.Tipo,
                request.Valor,
                request.Descricao,
                request.NumeroDocumento
            );
            await _caixaRepository.UpdateAsync(caixa, cancellationToken);
            _logger.LogInformation("Movimento registrado no caixa {CaixaId}", request.CaixaId);
            return Result<bool>.Success(true);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Erro de domínio ao registrar movimento no caixa {CaixaId}", request.CaixaId);
            return Result<bool>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao registrar movimento no caixa {CaixaId}", request.CaixaId);
            return Result<bool>.Failure("Erro ao registrar movimento");
        }
    }
}
