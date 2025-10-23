using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using EmpresaX.POS.Infrastructure.Data;
using EmpresaX.POS.Domain.Entities;
using EmpresaX.POS.API.Services.Exceptions;

namespace EmpresaX.POS.API.Services
{
    public class CaixaService : ICaixaService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CaixaService> _logger;

        public CaixaService(AppDbContext context, ILogger<CaixaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Caixa> FecharCaixaAsync(int caixaId, decimal valorInformado)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Iniciando fechamento do caixa {CaixaId}", caixaId);

                var caixa = await _context.Caixas.FirstOrDefaultAsync(c => c.Id == caixaId);

                if (caixa == null)
                {
                    throw new CaixaNaoEncontradoException($"Caixa com ID {caixaId} não foi encontrado.");
                }

                if (caixa.DataFechamento.HasValue)
                {
                    throw new CaixaJaFechadoException($"Caixa {caixaId} já foi fechado em {caixa.DataFechamento:dd/MM/yyyy HH:mm}.");
                }

                // Lógica de negócio para calcular o valor esperado (exemplo simplificado)
                // var totalVendas = await _context.Vendas.Where(v => v.CaixaId == caixaId).SumAsync(v => v.Total);
                var valorCalculado = caixa.ValorAbertura; // + totalVendas;

                caixa.DataFechamento = DateTime.UtcNow;
                caixa.ValorCalculadoFechamento = valorCalculado;
                caixa.ValorInformadoFechamento = valorInformado;
                caixa.Diferenca = valorInformado - valorCalculado;

                _context.Caixas.Update(caixa);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Caixa {CaixaId} fechado com sucesso. Diferença: {Diferenca:C}", caixaId, caixa.Diferenca);
                return caixa;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Erro ao fechar caixa {CaixaId}", caixaId);
                throw; // Re-lança a exceção para o controller tratar
            }
        }
    }
}

