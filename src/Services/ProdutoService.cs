using System.Collections.Generic;
using System.Threading.Tasks;
using EmpresaX.POS.API.Modelos.DTOs;

namespace EmpresaX.POS.API.Services
{
    public class ProdutoService : IProdutoService
    {
        // No futuro, você injetará seu DbContext aqui para falar com o banco de dados.
        // private readonly SeuDbContext _context;
        // public ProdutoService(SeuDbContext context) { _context = context; }

        public Task<ProdutoDto> CreateAsync(CreateProdutoDto produto)
        {
            // Lógica de criação (ainda com dados fixos)
            var novoProduto = new ProdutoDto
            {
                Id = new Random().Next(100, 1000), // ID aleatório por enquanto
                Nome = produto.Nome,
                Preco = produto.Preco,
                Estoque = produto.Estoque
            };
            return Task.FromResult(novoProduto);
        }

        public Task DeleteAsync(int id)
        {
            // Lógica de exclusão (simulada)
            if (id <= 0)
            {
                throw new KeyNotFoundException($"Produto com id {id} não encontrado.");
            }
            return Task.CompletedTask;
        }

        public Task<ProdutoDto?> GetByIdAsync(int id)
        {
            // Lógica de busca (ainda com dados fixos)
            if (id <= 0)
            {
                return Task.FromResult<ProdutoDto?>(null);
            }
            var produto = new ProdutoDto { Id = id, Nome = $"Produto {id}", Preco = 19.99m, Estoque = 50 };
            return Task.FromResult<ProdutoDto?>(produto);
        }

        public Task UpdateAsync(int id, UpdateProdutoDto produto)
        {
            // Lógica de atualização (simulada)
             if (id <= 0)
            {
                throw new KeyNotFoundException($"Produto com id {id} não encontrado.");
            }
            return Task.CompletedTask;
        }
    }
}
