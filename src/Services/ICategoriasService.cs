using EmpresaX.POS.API.Modelos.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmpresaX.POS.API.Services
{
    public interface ICategoriasService
    {
        Task<IEnumerable<CategoriaDto>> GetAllAsync();
        Task<CategoriaDto?> GetByIdAsync(int id);
        Task<CategoriaDto> CreateAsync(CreateCategoriaDto categoria);
        Task UpdateAsync(int id, CategoriaDto categoria); // Usaremos CategoriaDto para update
        Task DeleteAsync(int id);
    }
}