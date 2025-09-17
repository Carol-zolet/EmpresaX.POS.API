using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.API.Models.Finance
{
    public class FechamentoCaixa
    {
        public int Id { get; set; }

        [Required]
        public int ContaId { get; set; }

        [Required]
        public DateTime DataFechamento { get; set; }

        [Required]
        public decimal SaldoInicial { get; set; }

        [Required]
        public decimal TotalEntradas { get; set; }

        [Required]
        public decimal TotalSaidas { get; set; }

        [Required]
        public decimal SaldoFinal { get; set; }

        public decimal? SaldoInformado { get; set; }

        public decimal? Diferenca { get; set; }

        [Required]
        public StatusFechamento Status { get; set; }

        [StringLength(1000)]
        public string? Observacoes { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Conta Conta { get; set; } = null!;
    }

    public enum StatusFechamento
    {
        Aberto = 0,
        Fechado = 1,
        Conciliado = 2
    }
}
