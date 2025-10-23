using Microsoft.EntityFrameworkCore;
using EmpresaX.POS.Domain.Vendas;
using EmpresaX.POS.Domain.Vendas.Enums;
using EmpresaX.POS.Infrastructure.Data;

namespace EmpresaX.POS.Infrastructure.Repositories
{
    public class VendaRepository : IVendaRepository
    {
        private readonly AppDbContext _context;

        public VendaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Venda?> ObterPorIdAsync(Guid id) =>
            await _context.Vendas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.Id == id);

        public async Task<List<Venda>> ListarTodas() =>
            await _context.Vendas
                .Include(v => v.Cliente)
                .OrderByDescending(v => v.Data)
                .ToListAsync();

        public async Task<List<Venda>> ListarPorStatusAsync(StatusVenda status) =>
            await _context.Vendas
                .Include(v => v.Cliente)
                .Where(v => v.Status == status)
                .OrderByDescending(v => v.Data)
                .ToListAsync();

        public async Task<List<Venda>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim) =>
            await _context.Vendas
                .Include(v => v.Cliente)
                .Where(v => v.Data.Date >= inicio.Date && v.Data.Date <= fim.Date)
                .OrderByDescending(v => v.Data)
                .ToListAsync();

        public async Task<List<Venda>> ListarPorClienteAsync(Guid clienteId) =>
            await _context.Vendas
                .Include(v => v.Cliente)
                .Where(v => v.ClienteId == clienteId)
                .OrderByDescending(v => v.Data)
                .ToListAsync();

        public async Task<List<Venda>> ListarPorFormaPagamentoAsync(FormaPagamento forma) =>
            await _context.Vendas
                .Include(v => v.Cliente)
                .Where(v => v.FormaPagamento == forma)
                .OrderByDescending(v => v.Data)
                .ToListAsync();

        public async Task<List<Venda>> ListarVendasDoDiaAsync(DateTime data) =>
            await _context.Vendas
                .Include(v => v.Cliente)
                .Where(v => v.Data.Date == data.Date)
                .OrderByDescending(v => v.Data)
                .ToListAsync();

        public async Task<List<Venda>> ListarVendasDoMesAsync(int ano, int mes) =>
            await _context.Vendas
                .Include(v => v.Cliente)
                .Where(v => v.Data.Year == ano && v.Data.Month == mes)
                .OrderByDescending(v => v.Data)
                .ToListAsync();

        public async Task AdicionarAsync(Venda venda) =>
            await _context.Vendas.AddAsync(venda);

        public async Task SalvarAlteracoesAsync() =>
            await _context.SaveChangesAsync();

        public void Remover(Venda venda) =>
            _context.Vendas.Remove(venda);
    }
}
