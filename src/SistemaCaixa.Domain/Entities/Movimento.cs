using SistemaCaixa.Domain.ValueObjects;

namespace SistemaCaixa.Domain.Entities;

public class Movimento : Entity
{
    public Guid Id { get; private set; }
    public Guid CaixaId { get; private set; }
    public DateTime DataHora { get; private set; }
    public TipoMovimento Tipo { get; private set; }
    public Dinheiro Valor { get; private set; }
    public string Descricao { get; private set; }
    public string? NumeroDocumento { get; private set; }
    public bool Conciliado { get; private set; }
    public Guid? ConciliacaoId { get; private set; }

    // Navigation
    public Caixa Caixa { get; private set; }
    public Conciliacao? Conciliacao { get; private set; }

    private Movimento() { } // EF Core

    internal static Movimento Criar(
        Guid caixaId,
        TipoMovimento tipo,
        Dinheiro valor,
        string descricao,
        string? numeroDocumento)
    {
        return new Movimento
        {
            Id = Guid.NewGuid(),
            CaixaId = caixaId,
            DataHora = DateTime.UtcNow,
            Tipo = tipo,
            Valor = valor,
            Descricao = descricao?.Trim(),
            NumeroDocumento = numeroDocumento?.Trim(),
            Conciliado = false
        };
    }

    public void MarcarComoConciliado(Guid conciliacaoId)
    {
        if (Conciliado)
            throw new DomainException("Movimento já foi conciliado");

        Conciliado = true;
        ConciliacaoId = conciliacaoId;
    }
}
