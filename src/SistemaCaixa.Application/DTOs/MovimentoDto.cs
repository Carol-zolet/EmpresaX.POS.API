namespace SistemaCaixa.Application.DTOs;

public class MovimentoDto
{
    public Guid Id { get; set; }
    public Guid CaixaId { get; set; }
    public DateTime DataHora { get; set; }
    public string Tipo { get; set; }
    public decimal Valor { get; set; }
    public string Descricao { get; set; }
    public string? NumeroDocumento { get; set; }
    public bool Conciliado { get; set; }
}
