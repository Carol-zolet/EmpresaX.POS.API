using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaX.POS.Domain.Entities
{
    public class ContaBancaria
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Banco { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Agencia { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Conta { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Tipo { get; set; } = "Conta Corrente";
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoAtual { get; set; } = 0;
        
        public bool Ativa { get; set; } = true;
        
        [StringLength(20)]
        public string? ContaContabil { get; set; }
        
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        
        // Relacionamentos
        public virtual ICollection<MovimentacaoBancaria> Movimentacoes { get; set; } = new List<MovimentacaoBancaria>();
    }
}
