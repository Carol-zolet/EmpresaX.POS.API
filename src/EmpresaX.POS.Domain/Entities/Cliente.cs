using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;
        
        [StringLength(18)]
        public string? CnpjCpf { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        public bool Ativo { get; set; } = true;
        
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}


