using Microsoft.EntityFrameworkCore;
using SistemaCaixa.Domain.Entities;
using SistemaCaixa.Domain.Repositories;
using SistemaCaixa.Infrastructure.Data;

namespace SistemaCaixa.Infrastructure.Repositories;

public class ConciliacaoRepository : IConciliacaoRepository
{
    private readonly AppDbContext _context;
    public ConciliacaoRepository(AppDbContext context) => _context = context;

    public async Task<Conciliacao?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Conciliacoes.Include(c => c.Itens).FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task AddAsync(Conciliacao conciliacao, CancellationToken ct)
    {
        await _context.Conciliacoes.AddAsync(conciliacao, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Conciliacao conciliacao, CancellationToken ct)
    {
        _context.Conciliacoes.Update(conciliacao);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Conciliacao>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken ct)
        => await _context.Conciliacoes.Where(c => c.PeriodoInicio >= inicio && c.PeriodoFim <= fim).ToListAsync(ct);
}
