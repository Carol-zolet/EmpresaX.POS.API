using EmpresaX.POS.API.Models;

namespace EmpresaX.POS.API.Services
{
    public interface IConciliacaoService
    {
        Task<ConciliacaoBancaria> ProcessarArquivoBancarioAsync(IFormFile arquivo, string contaBancaria);
        Task<List<ItemConciliacao>> ObterItensPendentesAsync(int conciliacaoId);
        Task<bool> ConciliarItemManualAsync(int itemId, int contaPagarId);
        Task<decimal> CalcularScoreConfiancaAsync(ItemConciliacao itemBanco, dynamic contaPagar);
    }

    public class ConciliacaoService : IConciliacaoService
    {
        public Task<ConciliacaoBancaria> ProcessarArquivoBancarioAsync(IFormFile arquivo, string contaBancaria)
        {
            throw new NotImplementedException();
        }

        public Task<List<ItemConciliacao>> ObterItensPendentesAsync(int conciliacaoId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConciliarItemManualAsync(int itemId, int contaPagarId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> CalcularScoreConfiancaAsync(ItemConciliacao itemBanco, dynamic contaPagar)
        {
            throw new NotImplementedException();
        }
    }
}
