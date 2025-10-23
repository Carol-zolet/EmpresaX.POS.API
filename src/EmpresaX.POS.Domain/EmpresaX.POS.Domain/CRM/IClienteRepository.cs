using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmpresaX.POS.Domain.Shared.ValueObjects;
using EmpresaX.POS.Domain.CRM.Enums;

namespace EmpresaX.POS.Domain.CRM
{
    public interface IClienteRepository
    {
        Task<Cliente?> ObterPorIdAsync(Guid id);
        Task<Cliente?> ObterPorEmailAsync(Email email);
        Task<List<Cliente>> ListarTodosAsync();
        Task<List<Cliente>> ListarPorStatusAsync(StatusCliente status);
        Task<bool> ExisteComEmailAsync(Email email, Guid? ignorarId = null);
        Task AdicionarAsync(Cliente cliente);
        Task SalvarAlteracoesAsync();
        void Remover(Cliente cliente);
    }
}
