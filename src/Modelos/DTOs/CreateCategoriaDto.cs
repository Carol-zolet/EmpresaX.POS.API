using System.ComponentModel.DataAnnotations;
namespace EmpresaX.POS.API.Modelos.DTOs
{
    public class CreateCategoriaDto
    {
        [Required]
        public string? Nome { get; set; }
    }
}
