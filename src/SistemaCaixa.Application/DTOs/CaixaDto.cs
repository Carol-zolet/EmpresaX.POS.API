namespace SistemaCaixa.Application.DTOs;

public class CaixaDto
{
    public Guid Id { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime? DataFechamento { get; set; }
    public decimal SaldoInicial { get; set; }
    public decimal SaldoFinal { get; set; }
    public string Status { get; set; }
    public string OperadorAbertura { get; set; }
    public string? OperadorFechamento { get; set; }
    public int QuantidadeMovimentos { get; set; }
}
