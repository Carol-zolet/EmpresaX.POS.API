namespace SistemaCaixa.Application.DTOs;

public class RelatorioFechamentoDto
{
    public Guid CaixaId { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime DataFechamento { get; set; }
    public decimal SaldoInicial { get; set; }
    public decimal TotalEntradas { get; set; }
    public decimal TotalSaidas { get; set; }
    public decimal SaldoCalculado { get; set; }
    public decimal SaldoFinal { get; set; }
    public decimal Divergencia { get; set; }
    public int QuantidadeMovimentos { get; set; }
    public bool TemDivergencia { get; set; }
}
