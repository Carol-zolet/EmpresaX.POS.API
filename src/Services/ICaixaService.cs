using EmpresaX.POS.Domain.Entities;
using System.Threading.Tasks;

namespace EmpresaX.POS.API.Services
{
    public interface ICaixaService
    {
        // Por enquanto, vamos focar apenas no método que já temos a lógica
        Task<Caixa> FecharCaixaAsync(int caixaId, decimal valorInformado);
        // TODO: Adicionar outros métodos como AbrirCaixaAsync, etc.
    }
}

