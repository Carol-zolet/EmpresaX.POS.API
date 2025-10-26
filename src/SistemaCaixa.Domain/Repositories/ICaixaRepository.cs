using SistemaCaixa.Domain.Entities;

namespace SistemaCaixa.Domain.Repositories;

public interface ICaixaRepository
{
    Task<Caixa?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Caixa caixa, CancellationToken ct);
    Task UpdateAsync(Caixa caixa, CancellationToken ct);
    Task<IEnumerable<Caixa>> ListarAbertosAsync(CancellationToken ct);
}
