using SistemaCaixa.Domain.Entities;

namespace SistemaCaixa.Domain.Repositories;

public interface IConciliacaoRepository
{
    Task<Conciliacao?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Conciliacao conciliacao, CancellationToken ct);
    Task UpdateAsync(Conciliacao conciliacao, CancellationToken ct);
    Task<IEnumerable<Conciliacao>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken ct);
}
