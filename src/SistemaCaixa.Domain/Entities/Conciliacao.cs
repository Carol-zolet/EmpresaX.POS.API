using SistemaCaixa.Domain.ValueObjects;

namespace SistemaCaixa.Domain.Entities;

public class Conciliacao : Entity
{
    public Guid Id { get; private set; }
    public DateTime DataConciliacao { get; private set; }
    public DateTime PeriodoInicio { get; private set; }
    public DateTime PeriodoFim { get; private set; }
    public StatusConciliacao Status { get; private set; }
    public string ResponsavelConciliacao { get; private set; }
    
    private readonly List<ItemConciliacao> _itens = new();
    public IReadOnlyCollection<ItemConciliacao> Itens => _itens.AsReadOnly();

    public Dinheiro TotalSistema { get; private set; }
    public Dinheiro TotalBanco { get; private set; }
    public Dinheiro Divergencia => TotalSistema.Subtrair(TotalBanco);
    public bool TemDivergencia => Math.Abs(Divergencia.Valor) > 0.01m;

    private Conciliacao() { } // EF Core

    public static Conciliacao Criar(
        DateTime periodoInicio,
        DateTime periodoFim,
        string responsavel)
    {
        if (periodoInicio >= periodoFim)
            throw new DomainException("Período inválido");

        return new Conciliacao
        {
            Id = Guid.NewGuid(),
            DataConciliacao = DateTime.UtcNow,
            PeriodoInicio = periodoInicio,
            PeriodoFim = periodoFim,
            Status = StatusConciliacao.EmAndamento,
            ResponsavelConciliacao = responsavel,
            TotalSistema = Dinheiro.Zero,
            TotalBanco = Dinheiro.Zero
        };
    }

    public void AdicionarItem(
        Movimento movimento,
        LancamentoBancario? lancamentoBancario,
        TipoItemConciliacao tipoItem)
    {
        var item = new ItemConciliacao
        {
            Id = Guid.NewGuid(),
            ConciliacaoId = Id,
            MovimentoId = movimento?.Id,
            LancamentoBancarioId = lancamentoBancario?.Id,
            TipoItem = tipoItem,
            ValorSistema = movimento?.Valor ?? Dinheiro.Zero,
            ValorBanco = lancamentoBancario?.Valor ?? Dinheiro.Zero,
            Observacao = GerarObservacao(tipoItem, movimento, lancamentoBancario)
        };

        _itens.Add(item);
        RecalcularTotais();
    }

    public void Finalizar()
    {
        if (Status == StatusConciliacao.Concluida)
            throw new DomainException("Conciliação já foi finalizada");

        Status = !TemDivergencia 
            ? StatusConciliacao.Concluida 
            : StatusConciliacao.ConcluidaComDivergencia;
    }

    private void RecalcularTotais()
    {
        TotalSistema = Dinheiro.FromBRL(
            _itens.Where(i => i.MovimentoId.HasValue).Sum(i => i.ValorSistema.Valor)
        );

        TotalBanco = Dinheiro.FromBRL(
            _itens.Where(i => i.LancamentoBancarioId.HasValue).Sum(i => i.ValorBanco.Valor)
        );
    }

    private string GerarObservacao(
        TipoItemConciliacao tipo,
        Movimento? movimento,
        LancamentoBancario? lancamento)
    {
        return tipo switch
        {
            TipoItemConciliacao.Conciliado => "Item conciliado automaticamente",
            TipoItemConciliacao.SomenteNoSistema => $"Movimento {movimento?.NumeroDocumento} não encontrado no banco",
            TipoItemConciliacao.SomenteNoBanco => $"Lançamento {lancamento?.NumeroDocumento} não encontrado no sistema",
            TipoItemConciliacao.DiferencaValor => $"Diferença de valor: Sistema {movimento?.Valor} vs Banco {lancamento?.Valor}",
            _ => string.Empty
        };
    }
}

public enum StatusConciliacao
{
    EmAndamento = 1,
    Concluida = 2,
    ConcluidaComDivergencia = 3,
    Cancelada = 4
}

public enum TipoItemConciliacao
{
    Conciliado = 1,
    SomenteNoSistema = 2,
    SomenteNoBanco = 3,
    DiferencaValor = 4
}
