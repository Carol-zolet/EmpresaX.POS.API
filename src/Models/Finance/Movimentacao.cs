using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.API.Models.Finance
{
    public class Movimentacao
    {
        public int Id { get; set; }

        [Required]
        public int ContaId { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public TipoMovimentacao Tipo { get; set; }

        [Required]
        public decimal Valor { get; set; }

        [Required]
        public DateTime DataMovimentacao { get; set; }

        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Observacoes { get; set; }

        public bool Conciliado { get; set; } = false;

        [StringLength(100)]
        public string? NumeroDocumento { get; set; }

        [StringLength(50)]
        public string Origem { get; set; } = "MANUAL";

        public DateTime DataRegistro { get; set; } = DateTime.Now;

        // Navigation Properties - remover required
        public virtual Conta? Conta { get; set; }
        public virtual Categoria? Categoria { get; set; }
    }

    public enum TipoMovimentacao
    {
        Entrada = 0,
        Saida = 1
    }
}
