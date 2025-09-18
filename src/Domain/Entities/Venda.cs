using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaX.POS.Domain.Entities
{
    public class Venda
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime Data { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Finalizada";
        
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
