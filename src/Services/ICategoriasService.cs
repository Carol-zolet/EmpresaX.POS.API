using EmpresaX.POS.API.Modelos.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmpresaX.POS.API.Services
{
    public interface ICategoriasService
    {
        Task<IEnumerable<CategoriaDto>> GetAllAsync();
    }
}
