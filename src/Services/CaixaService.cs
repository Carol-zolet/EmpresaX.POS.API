using EmpresaX.POS.API.Models;
using EmpresaX.POS.API.Data;
using Microsoft.EntityFrameworkCore;

namespace EmpresaX.POS.API.Services
{
    public interface ICaixaService
    {
        Task<FechamentoCaixa> AbrirCaixaAsync(string usuario, decimal saldoInicial);
        Task<FechamentoCaixa> FecharCaixaAsync(int caixaId, decimal saldoFinalInformado, string? observacoes = null);
        Task<MovimentacaoCaixa> AdicionarMovimentacaoAsync(int caixaId, TipoMovimento tipo, decimal valor, string descricao, string? categoria = null);
        Task<FechamentoCaixa?> ObterCaixaAbertoAsync(string usuario);
        Task<List<FechamentoCaixa>> ObterHistoricoCaixaAsync(DateTime dataInicio, DateTime dataFim);
        Task<decimal> CalcularSaldoAtualAsync(int caixaId);
    }

    public class CaixaService : ICaixaService
    {
        private readonly FinanceiroDbContext _context;

        public CaixaService(FinanceiroDbContext context)
        {
            _context = context;
        }

        public async Task<FechamentoCaixa> AbrirCaixaAsync(string usuario, decimal saldoInicial)
        {
            var caixaAberto = await _context.FechamentosCaixa
                .FirstOrDefaultAsync(c => c.Usuario == usuario && c.Status == StatusCaixa.Aberto);

            if (caixaAberto != null)
            {
                throw new InvalidOperationException("Usuário já possui um caixa aberto");
            }

            var novoCaixa = new FechamentoCaixa
            {
                DataAbertura = DateTime.Now,
                SaldoInicial = saldoInicial,
                SaldoFinal = saldoInicial,
                Usuario = usuario,
                Status = StatusCaixa.Aberto,
                TotalEntradas = 0,
                TotalSaidas = 0,
                DiferenciaCaixa = 0
            };

            _context.FechamentosCaixa.Add(novoCaixa);
            await _context.SaveChangesAsync();

            return novoCaixa;
        }

        public async Task<FechamentoCaixa> FecharCaixaAsync(int caixaId, decimal saldoFinalInformado, string? observacoes = null)
        {
            var caixa = await _context.FechamentosCaixa
                .FirstOrDefaultAsync(c => c.Id == caixaId);

            if (caixa == null)
                throw new ArgumentException("Caixa não encontrado");

            if (caixa.Status != StatusCaixa.Aberto)
                throw new InvalidOperationException("Caixa não está aberto");

            var saldoCalculado = await CalcularSaldoAtualAsync(caixaId);
            
            caixa.DataFechamento = DateTime.Now;
            caixa.SaldoFinal = saldoCalculado;
            caixa.DiferenciaCaixa = saldoFinalInformado - saldoCalculado;
            caixa.Status = StatusCaixa.Fechado;
            caixa.Observacoes = observacoes;

            await _context.SaveChangesAsync();
            return caixa;
        }

        public async Task<MovimentacaoCaixa> AdicionarMovimentacaoAsync(int caixaId, TipoMovimento tipo, decimal valor, string descricao, string? categoria = null)
        {
            var caixa = await _context.FechamentosCaixa
                .FirstOrDefaultAsync(c => c.Id == caixaId);

            if (caixa == null)
                throw new ArgumentException("Caixa não encontrado");

            if (caixa.Status != StatusCaixa.Aberto)
                throw new InvalidOperationException("Caixa não está aberto");

            var movimentacao = new MovimentacaoCaixa
            {
                FechamentoCaixaId = caixaId,
                DataHora = DateTime.Now,
                Tipo = tipo,
                Valor = Math.Abs(valor),
                Descricao = descricao,
                Categoria = categoria
            };

            _context.MovimentacoesCaixa.Add(movimentacao);

            if (tipo == TipoMovimento.Entrada)
                caixa.TotalEntradas += movimentacao.Valor;
            else
                caixa.TotalSaidas += movimentacao.Valor;

            await _context.SaveChangesAsync();
            return movimentacao;
        }

        public async Task<FechamentoCaixa?> ObterCaixaAbertoAsync(string usuario)
        {
            return await _context.FechamentosCaixa
                .Include(c => c.Movimentacoes)
                .FirstOrDefaultAsync(c => c.Usuario == usuario && c.Status == StatusCaixa.Aberto);
        }

        public async Task<List<FechamentoCaixa>> ObterHistoricoCaixaAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.FechamentosCaixa
                .Include(c => c.Movimentacoes)
                .Where(c => c.DataAbertura >= dataInicio && c.DataAbertura <= dataFim)
                .ToListAsync();
        }

        public async Task<decimal> CalcularSaldoAtualAsync(int caixaId)
        {
            var caixa = await _context.FechamentosCaixa
                .FirstOrDefaultAsync(c => c.Id == caixaId);

            if (caixa == null) return 0;

            return caixa.SaldoInicial + caixa.TotalEntradas - caixa.TotalSaidas;
        }
    }
}
