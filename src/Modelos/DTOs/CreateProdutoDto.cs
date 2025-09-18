using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.API.Modelos.DTOs
{
    public class CreateProdutoDto
    {
        [Required(ErrorMessage = "O nome do produto � obrigat�rio.")]
        public string? Nome { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O pre�o deve ser maior que zero.")]
        public decimal Preco { get; set; }

        public int Estoque { get; set; }
    }
}
