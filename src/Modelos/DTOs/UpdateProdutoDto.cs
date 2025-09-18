using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.API.Modelos.DTOs
{
    public class UpdateProdutoDto
    {
       
        public string? Nome { get; set; }

       
        public decimal Preco { get; set; }

        public int Estoque { get; set; }
    }
}
