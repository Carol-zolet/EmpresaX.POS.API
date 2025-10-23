using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaX.POS.Domain.Entities
{
    public class ContaReceber
    {
        public int Id { get; set; }
        
        [Required]
        public int ClienteId { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
        
        [Required]
        public DateTime DataVencimento { get; set; }
        
        public DateTime? DataRecebimento { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Pendente"; // Pendente, Recebida, Vencida
        
        [StringLength(100)]
        public string? NumeroDocumento { get; set; }
        
        [StringLength(1000)]
        public string? Observacoes { get; set; }
        
        public int? VendaId { get; set; }
        
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        
        // Relacionamentos
        public virtual Cliente Cliente { get; set; } = null!;
        public virtual Venda? Venda { get; set; }
    }
}


