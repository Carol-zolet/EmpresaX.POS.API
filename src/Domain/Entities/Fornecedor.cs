using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.Domain.Entities
{
    public class Fornecedor
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;
        
        [StringLength(18)]
        public string? CnpjCpf { get; set; }
        
        [StringLength(500)]
        public string? Endereco { get; set; }
        
        [StringLength(20)]
        public string? Telefone { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        public bool Ativo { get; set; } = true;
        
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        
        // Relacionamentos
        public virtual ICollection<ContaPagar> ContasPagar { get; set; } = new List<ContaPagar>();
    }
}


