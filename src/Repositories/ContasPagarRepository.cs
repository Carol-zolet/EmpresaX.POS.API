using EmpresaX.POS.API.Data;
using Microsoft.EntityFrameworkCore;

namespace EmpresaX.POS.API.Repositories
{
    public interface IContasPagarRepository
    {
        Task<IEnumerable<ContaPagarEntity>> GetAllAsync();
        Task<ContaPagarEntity?> GetByIdAsync(int id);
        Task<ContaPagarEntity> AddAsync(ContaPagarEntity conta);
        Task<IEnumerable<ContaPagarEntity>> AddRangeAsync(IEnumerable<ContaPagarEntity> contas);
        Task UpdateAsync(ContaPagarEntity conta);
        Task DeleteAsync(int id);
        Task ClearAllAsync();
        Task<decimal> GetTotalAsync();
        Task<int> GetCountAsync();
        Task<decimal> GetTotalByStatusAsync(string status);
        Task<int> GetCountByStatusAsync(string status);
    }

    public class ContasPagarRepository : IContasPagarRepository
    {
        private readonly EmpresaXDbContext _context;

        public ContasPagarRepository(EmpresaXDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContaPagarEntity>> GetAllAsync()
        {
            return await _context.ContasPagar.OrderByDescending(c => c.CreatedAt).ToListAsync();
        }

        public async Task<ContaPagarEntity?> GetByIdAsync(int id)
        {
            return await _context.ContasPagar.FindAsync(id);
        }

        public async Task<ContaPagarEntity> AddAsync(ContaPagarEntity conta)
        {
            conta.CreatedAt = DateTime.Now;
            conta.UpdatedAt = DateTime.Now;
            _context.ContasPagar.Add(conta);
            await _context.SaveChangesAsync();
            return conta;
        }

        public async Task<IEnumerable<ContaPagarEntity>> AddRangeAsync(IEnumerable<ContaPagarEntity> contas)
        {
            var contasList = contas.ToList();
            var agora = DateTime.Now;
            
            foreach (var conta in contasList)
            {
                conta.CreatedAt = agora;
                conta.UpdatedAt = agora;
            }
            
            _context.ContasPagar.AddRange(contasList);
            await _context.SaveChangesAsync();
            return contasList;
        }

        public async Task UpdateAsync(ContaPagarEntity conta)
        {
            conta.UpdatedAt = DateTime.Now;
            _context.ContasPagar.Update(conta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var conta = await _context.ContasPagar.FindAsync(id);
            if (conta != null)
            {
                _context.ContasPagar.Remove(conta);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearAllAsync()
        {
            var contas = await _context.ContasPagar.ToListAsync();
            _context.ContasPagar.RemoveRange(contas);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalAsync()
        {
            return await _context.ContasPagar.SumAsync(c => c.Valor);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.ContasPagar.CountAsync();
        }

        public async Task<decimal> GetTotalByStatusAsync(string status)
        {
            return await _context.ContasPagar
                .Where(c => c.Status == status)
                .SumAsync(c => c.Valor);
        }

        public async Task<int> GetCountByStatusAsync(string status)
        {
            return await _context.ContasPagar
                .Where(c => c.Status == status)
                .CountAsync();
        }
    }
}
