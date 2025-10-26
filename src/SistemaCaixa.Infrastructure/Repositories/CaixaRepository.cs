using Microsoft.EntityFrameworkCore;
using SistemaCaixa.Domain.Entities;
using SistemaCaixa.Domain.Repositories;
using SistemaCaixa.Infrastructure.Data;

namespace SistemaCaixa.Infrastructure.Repositories;

public class CaixaRepository : ICaixaRepository
{
    private readonly AppDbContext _context;
    public CaixaRepository(AppDbContext context) => _context = context;

    public async Task<Caixa?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Caixas.Include(c => c.Movimentos).FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task AddAsync(Caixa caixa, CancellationToken ct)
    {
        await _context.Caixas.AddAsync(caixa, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Caixa caixa, CancellationToken ct)
    {
        _context.Caixas.Update(caixa);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Caixa>> ListarAbertosAsync(CancellationToken ct)
        => await _context.Caixas.Where(c => c.Status == StatusCaixa.Aberto).ToListAsync(ct);
}
