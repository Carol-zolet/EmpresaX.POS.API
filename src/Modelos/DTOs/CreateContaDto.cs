using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.API.Modelos.DTOs
{
    public class CreateContaDto
    {
        [Required]
        public string? Descricao { get; set; }
        
        [Range(0.01, double.MaxValue)]
        public decimal Valor { get; set; }
        
        public DateTime DataVencimento { get; set; }
    }
}
