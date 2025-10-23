using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaX.POS.Domain.Entities
{
    public class CentroCusto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(10)]
        public string Codigo { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Descricao { get; set; }
        
        [StringLength(100)]
        public string? Responsavel { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OrcamentoMensal { get; set; }
        
        public bool Ativo { get; set; } = true;
        
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        
        // Relacionamentos
        public virtual ICollection<ContaPagar> ContasPagar { get; set; } = new List<ContaPagar>();
    }
}


