using SistemaCaixa.Domain.ValueObjects;
using SistemaCaixa.Domain.Events;

namespace SistemaCaixa.Domain.Entities;

public class Caixa : Entity
{
    public Guid Id { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public DateTime? DataFechamento { get; private set; }
    public Dinheiro SaldoInicial { get; private set; }
    public Dinheiro SaldoFinal { get; private set; }
    public StatusCaixa Status { get; private set; }
    public string OperadorAbertura { get; private set; }
    public string? OperadorFechamento { get; private set; }
    
    private readonly List<Movimento> _movimentos = new();
    public IReadOnlyCollection<Movimento> Movimentos => _movimentos.AsReadOnly();

    // Domain Events
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Caixa() { } // EF Core

    public static Caixa Abrir(Dinheiro saldoInicial, string operador)
    {
        if (saldoInicial.EhNegativo())
            throw new DomainException("Saldo inicial não pode ser negativo");

        if (string.IsNullOrWhiteSpace(operador))
            throw new DomainException("Operador é obrigatório");

        var caixa = new Caixa
        {
            Id = Guid.NewGuid(),
            DataAbertura = DateTime.UtcNow,
            SaldoInicial = saldoInicial,
            SaldoFinal = saldoInicial,
            Status = StatusCaixa.Aberto,
            OperadorAbertura = operador
        };

        caixa.AdicionarEvento(new CaixaAberto(caixa.Id, operador, saldoInicial));

        return caixa;
    }

    public void RegistrarMovimento(
        TipoMovimento tipo,
        Dinheiro valor,
        string descricao,
        string? numeroDocumento = null)
    {
        ValidarCaixaAberto();

        if (valor.EhNegativo() || valor.Valor == 0)
            throw new DomainException("Valor do movimento deve ser positivo");

        var movimento = Movimento.Criar(
            caixaId: Id,
            tipo: tipo,
            valor: valor,
            descricao: descricao,
            numeroDocumento: numeroDocumento
        );

        _movimentos.Add(movimento);
        
        // Atualizar saldo
        SaldoFinal = tipo == TipoMovimento.Entrada
            ? SaldoFinal.Somar(valor)
            : SaldoFinal.Subtrair(valor);

        AdicionarEvento(new MovimentoRegistrado(Id, movimento.Id, tipo, valor));
    }

    public RelatorioFechamento Fechar(string operador)
    {
        ValidarCaixaAberto();

        DataFechamento = DateTime.UtcNow;
        Status = StatusCaixa.Fechado;
        OperadorFechamento = operador;

        var relatorio = GerarRelatorioFechamento();

        AdicionarEvento(new CaixaFechado(Id, operador, SaldoFinal, relatorio));

        return relatorio;
    }

    private RelatorioFechamento GerarRelatorioFechamento()
    {
        var entradas = _movimentos
            .Where(m => m.Tipo == TipoMovimento.Entrada)
            .Sum(m => m.Valor.Valor);

        var saidas = _movimentos
            .Where(m => m.Tipo == TipoMovimento.Saida)
            .Sum(m => m.Valor.Valor);

        var saldoCalculado = SaldoInicial.Valor + entradas - saidas;
        var divergencia = SaldoFinal.Valor - saldoCalculado;

        return new RelatorioFechamento
        {
            CaixaId = Id,
            DataAbertura = DataAbertura,
            DataFechamento = DataFechamento!.Value,
            SaldoInicial = SaldoInicial,
            TotalEntradas = Dinheiro.FromBRL(entradas),
            TotalSaidas = Dinheiro.FromBRL(saidas),
            SaldoCalculado = Dinheiro.FromBRL(saldoCalculado),
            SaldoFinal = SaldoFinal,
            Divergencia = Dinheiro.FromBRL(divergencia),
            QuantidadeMovimentos = _movimentos.Count,
            TemDivergencia = Math.Abs(divergencia) > 0.01m
        };
    }

    private void ValidarCaixaAberto()
    {
        if (Status != StatusCaixa.Aberto)
            throw new DomainException($"Caixa não está aberto. Status atual: {Status}");
    }

    private void AdicionarEvento(IDomainEvent evento)
    {
        _domainEvents.Add(evento);
    }

    public void LimparEventos() => _domainEvents.Clear();
}

public enum StatusCaixa
{
    Aberto = 1,
    Fechado = 2,
    Auditado = 3
}

public enum TipoMovimento
{
    Entrada = 1,
    Saida = 2
}
