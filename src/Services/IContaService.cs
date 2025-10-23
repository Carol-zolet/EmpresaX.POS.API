// Agora o 'using' aponta para o namespace correto dos DTOs
using EmpresaX.POS.API.Modelos.DTOs; 

namespace EmpresaX.POS.API.Services
{
    public interface IContaService
    {
        Task<IEnumerable<ContaDto>> GetAllAsync();
        Task<ContaDto?> GetByIdAsync(int id);
        Task<ContaDto> CreateAsync(CreateContaDto conta);
    Task UpdateAsync(int id, ContaDto conta);
    Task DeleteAsync(int id);
    }
}


