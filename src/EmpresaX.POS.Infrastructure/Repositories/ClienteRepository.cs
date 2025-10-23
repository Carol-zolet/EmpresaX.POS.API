using Microsoft.EntityFrameworkCore;
using EmpresaX.POS.Domain.CRM;
using EmpresaX.POS.Domain.Shared.ValueObjects;
using EmpresaX.POS.Infrastructure.Data;
using EmpresaX.POS.Domain.CRM.Enums;

namespace EmpresaX.POS.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente?> ObterPorIdAsync(Guid id) =>
            await _context.Clientes.FindAsync(id);

        public async Task<Cliente?> ObterPorEmailAsync(Email email) =>
            await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == email);

        public async Task<List<Cliente>> ListarTodosAsync() =>
            await _context.Clientes
                .OrderBy(c => c.Nome)
                .ToListAsync();

        public async Task<List<Cliente>> ListarPorStatusAsync(StatusCliente status) =>
            await _context.Clientes
                .Where(c => c.Status == status)
                .OrderBy(c => c.Nome)
                .ToListAsync();

        public async Task<bool> ExisteComEmailAsync(Email email, Guid? ignorarId = null)
        {
            var query = _context.Clientes.Where(c => c.Email == email);
            
            if (ignorarId.HasValue)
                query = query.Where(c => c.Id != ignorarId.Value);

            return await query.AnyAsync();
        }

        public async Task AdicionarAsync(Cliente cliente) =>
            await _context.Clientes.AddAsync(cliente);

        public async Task SalvarAlteracoesAsync() =>
            await _context.SaveChangesAsync();

        public void Remover(Cliente cliente) =>
            _context.Clientes.Remove(cliente);
    }
}
