using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.API.Modelos.DTOs
{
    public class CreateContaDto
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "A descrição deve ter entre 5 e 500 caracteres")]
        public string Descricao { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "O valor é obrigatório")]
        [Range(0.01, 1000000, ErrorMessage = "O valor deve estar entre R$ 0,01 e R$ 1.000.000,00")]
        public decimal Valor { get; set; }
        
        [Required(ErrorMessage = "A data de vencimento é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataVencimento { get; set; }

        [StringLength(200, ErrorMessage = "O fornecedor deve ter no máximo 200 caracteres")]
        public string? Fornecedor { get; set; }

        [StringLength(100, ErrorMessage = "A categoria deve ter no máximo 100 caracteres")]
        public string? Categoria { get; set; }
    }
}


