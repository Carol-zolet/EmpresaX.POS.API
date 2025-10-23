using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaX.POS.Domain.Entities
{
    public class MovimentacaoBancaria
    {
        public int Id { get; set; }
        
        [Required]
        public int ContaBancariaId { get; set; }
        
        [Required]
        public DateTime Data { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Historico { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? NumeroDocumento { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Tipo { get; set; } = string.Empty; // Débito, Crédito
        
        public bool Conciliado { get; set; } = false;
        
        public int? LancamentoContabilId { get; set; }
        
        [StringLength(1000)]
        public string? Observacoes { get; set; }
        
        public DateTime DataImportacao { get; set; } = DateTime.Now;
        
        // Relacionamentos
        public virtual ContaBancaria ContaBancaria { get; set; } = null!;
    }
}


