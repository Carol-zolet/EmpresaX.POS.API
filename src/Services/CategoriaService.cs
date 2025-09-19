using EmpresaX.POS.API.Modelos.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmpresaX.POS.API.Services
{
    public class CategoriaService : ICategoriasService
    {
        public Task<IEnumerable<CategoriaDto>> GetAllAsync()
        {
            // Lógica de placeholder
            var lista = new List<CategoriaDto> { new CategoriaDto { Id = 1, Nome = "Categoria Falsa" } };
            return Task.FromResult<IEnumerable<CategoriaDto>>(lista);
        }
    }
}
