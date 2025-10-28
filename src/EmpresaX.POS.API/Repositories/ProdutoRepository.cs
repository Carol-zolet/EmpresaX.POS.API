using EmpresaX.POS.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmpresaX.POS.API.Repositories
{
    public interface IProdutoRepository
    {
    Task<List<ProdutoDto>> GetAllAsync();
    Task<ProdutoDto?> GetByIdAsync(int id);
    Task<ProdutoDto> AddAsync(CreateProdutoRequest request);
    Task<ProdutoDto?> UpdateAsync(int id, UpdateProdutoRequest request);
        Task<bool> DeleteAsync(int id);
    }

    public class ProdutoRepository : IProdutoRepository
    {
        // Simulação de dados em memória
        private static readonly List<ProdutoDto> _produtos = new();
        private static int _nextId = 1;

        public Task<List<ProdutoDto>> GetAllAsync() => Task.FromResult(_produtos);

        public Task<ProdutoDto?> GetByIdAsync(int id) =>
            Task.FromResult(_produtos.Find(p => p.Id == id));

        public Task<ProdutoDto> AddAsync(CreateProdutoRequest request)
        {
            var produto = new ProdutoDto
            {
                Id = _nextId++,
                Nome = request.Nome,
                Preco = request.Preco,
                Estoque = request.Estoque
            };
            _produtos.Add(produto);
            return Task.FromResult(produto);
        }

        public Task<ProdutoDto?> UpdateAsync(int id, UpdateProdutoRequest request)
        {
            var produto = _produtos.Find(p => p.Id == id);
            if (produto == null) return Task.FromResult<ProdutoDto?>(null);
            produto.Nome = request.Nome;
            produto.Preco = request.Preco;
            produto.Estoque = request.Estoque;
            return Task.FromResult<ProdutoDto?>(produto);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var produto = _produtos.Find(p => p.Id == id);
            if (produto == null) return Task.FromResult(false);
            _produtos.Remove(produto);
            return Task.FromResult(true);
        }
    }
}