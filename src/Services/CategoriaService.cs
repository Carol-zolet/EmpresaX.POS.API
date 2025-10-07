using EmpresaX.POS.API.Modelos.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmpresaX.POS.API.Services
{
    public class CategoriaService : ICategoriaService
    {
        // Simulação de dados em memória
        private readonly List<CategoriaDto> _categorias = new List<CategoriaDto>
        {
            new CategoriaDto { Id = 1, Nome = "Bebidas" },
            new CategoriaDto { Id = 2, Nome = "Lanches" }
        };
        private int _nextId = 3;

        public Task<IEnumerable<CategoriaDto>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<CategoriaDto>>(_categorias);
        }

        public Task<CategoriaDto?> GetByIdAsync(int id)
        {
            var categoria = _categorias.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(categoria);
        }

        public Task<CategoriaDto> CreateAsync(CreateCategoriaDto categoriaDto)
        {
            var novaCategoria = new CategoriaDto
            {
                Id = _nextId++,
                Nome = categoriaDto.Nome
            };
            _categorias.Add(novaCategoria);
            return Task.FromResult(novaCategoria);
        }

        public Task UpdateAsync(int id, CategoriaDto categoriaDto)
        {
            var categoriaExistente = _categorias.FirstOrDefault(c => c.Id == id);
            if (categoriaExistente == null)
            {
                throw new KeyNotFoundException($"Categoria com id {id} não encontrada.");
            }
            categoriaExistente.Nome = categoriaDto.Nome;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var categoriaExistente = _categorias.FirstOrDefault(c => c.Id == id);
             if (categoriaExistente == null)
            {
                throw new KeyNotFoundException($"Categoria com id {id} não encontrada.");
            }
            _categorias.Remove(categoriaExistente);
            return Task.CompletedTask;
        }
    }
}
