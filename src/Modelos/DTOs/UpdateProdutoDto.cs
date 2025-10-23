using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.API.Modelos.DTOs
{
    public class UpdateProdutoDto
    {
        [Required(ErrorMessage = "O ID é obrigatório")]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres")]
        public string? Nome { get; set; }

        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres")]
        public string? Descricao { get; set; }

        [Range(0.01, 999999.99, ErrorMessage = "O preço deve estar entre R$ 0,01 e R$ 999.999,99")]
        public decimal? Preco { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo")]
        public int? Estoque { get; set; }

        [StringLength(50, ErrorMessage = "O código de barras deve ter no máximo 50 caracteres")]
        public string? CodigoBarras { get; set; }

        public int? CategoriaId { get; set; }
    }
}


