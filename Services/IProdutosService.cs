using EmpresaX.POS.API.Modelos.DTOs;
using System.Threading.Tasks;

namespace EmpresaX.POS.API.Services
{
    public interface IProdutosService
    {
        // Adicionamos a assinatura do m�todo que o teste precisa
        Task<ProdutoDto?> GetByIdAsync(int id);
    }
}