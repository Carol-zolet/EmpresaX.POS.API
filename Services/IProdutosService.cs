using EmpresaX.POS.API.Modelos.DTOs;
using System.Threading.Tasks;

namespace EmpresaX.POS.API.Services
{
    public interface IProdutosService
    {
        // Adicionamos a assinatura do método que o teste precisa
        Task<ProdutoDto?> GetByIdAsync(int id);
    }
}