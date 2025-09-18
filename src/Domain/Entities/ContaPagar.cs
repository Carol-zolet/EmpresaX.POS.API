using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaX.POS.Domain.Entities
{
    public class ContaPagar
    {
        public int Id { get; set; }
        
        [Required]
        public int FornecedorId { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
        
        [Required]
        public DateTime DataVencimento { get; set; }
        
        public DateTime? DataPagamento { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Pendente"; // Pendente, Paga, Vencida
        
        [StringLength(100)]
        public string? Categoria { get; set; }
        
        [StringLength(100)]
        public string? NumeroDocumento { get; set; }
        
        [StringLength(1000)]
        public string? Observacoes { get; set; }
        
        public int? CentroCustoId { get; set; }
        
        [StringLength(20)]
        public string? ContaContabil { get; set; }
        
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        
        // Relacionamentos
        public virtual Fornecedor Fornecedor { get; set; } = null!;
        public virtual CentroCusto? CentroCusto { get; set; }
    }
}
