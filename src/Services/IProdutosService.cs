using EmpresaX.POS.API.Modelos.DTOs;
using System.Threading.Tasks;

namespace EmpresaX.POS.API.Services
{
    public interface IProdutosService
    {
        Task<ProdutoDto?> GetByIdAsync(int id);
        Task<ProdutoDto> CreateAsync(CreateProdutoDto produto);
        Task UpdateAsync(int id, UpdateProdutoDto produto);
        Task DeleteAsync(int id); // Garante que esta linha existe
    }
}