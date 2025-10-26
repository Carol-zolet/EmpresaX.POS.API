namespace SistemaCaixa.Application.DTOs;

public class ConciliacaoDto
{
    public Guid Id { get; set; }
    public DateTime DataConciliacao { get; set; }
    public DateTime PeriodoInicio { get; set; }
    public DateTime PeriodoFim { get; set; }
    public string Status { get; set; }
    public string ResponsavelConciliacao { get; set; }
    public decimal TotalSistema { get; set; }
    public decimal TotalBanco { get; set; }
    public decimal Divergencia { get; set; }
    public bool TemDivergencia { get; set; }
}
