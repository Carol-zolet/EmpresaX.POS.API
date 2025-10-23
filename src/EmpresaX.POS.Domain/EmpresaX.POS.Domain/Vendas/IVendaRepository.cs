using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmpresaX.POS.Domain.Vendas.Enums;

namespace EmpresaX.POS.Domain.Vendas
{
    public interface IVendaRepository
    {
        Task<Venda?> ObterPorIdAsync(Guid id);
        Task<List<Venda>> ListarTodas();
        Task<List<Venda>> ListarPorStatusAsync(StatusVenda status);
        Task<List<Venda>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim);
        Task<List<Venda>> ListarPorClienteAsync(Guid clienteId);
        Task<List<Venda>> ListarPorFormaPagamentoAsync(FormaPagamento forma);
        Task<List<Venda>> ListarVendasDoDiaAsync(DateTime data);
        Task<List<Venda>> ListarVendasDoMesAsync(int ano, int mes);
        Task AdicionarAsync(Venda venda);
        Task SalvarAlteracoesAsync();
        void Remover(Venda venda);
    }
}
