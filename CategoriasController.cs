using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmpresaX.POS.API
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoriasController : ControllerBase
	{
		private readonly ILogger<CategoriasController> _logger;
		// Simulação de dependência de serviço (substitua por ICategoriaService real)
		// private readonly ICategoriaService _categoriaService;

		public CategoriasController(ILogger<CategoriasController> logger)
		{
			_logger = logger;
			// _categoriaService = categoriaService;
		}

		// DTO de exemplo
		public class CategoriaDto
		{
			public int Id { get; set; }
			public string Nome { get; set; }
		}

		/// <summary>
		/// Lista todas as categorias
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAllAsync(CancellationToken ct)
		{
			try
			{
				// var categorias = await _categoriaService.GetAllAsync(ct);
				var categorias = new List<CategoriaDto> {
					new CategoriaDto { Id = 1, Nome = "Bebidas" },
					new CategoriaDto { Id = 2, Nome = "Alimentos" }
				};
				return Ok(categorias);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erro ao listar categorias");
				return StatusCode(500, "Erro interno ao buscar categorias");
			}
		}

		/// <summary>
		/// Busca uma categoria por ID
		/// </summary>
		[HttpGet("{id}")]
		public async Task<ActionResult<CategoriaDto>> GetByIdAsync(int id, CancellationToken ct)
		{
			try
			{
				// var categoria = await _categoriaService.GetByIdAsync(id, ct);
				var categoria = id == 1 ? new CategoriaDto { Id = 1, Nome = "Bebidas" } : null;
				if (categoria == null)
				{
					_logger.LogWarning("Categoria não encontrada: {CategoriaId}", id);
					return NotFound();
				}
				return Ok(categoria);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erro ao buscar categoria {CategoriaId}", id);
				return StatusCode(500, "Erro interno ao buscar categoria");
			}
		}

		/// <summary>
		/// Cria uma nova categoria
		/// </summary>
		[HttpPost]
		public async Task<ActionResult<CategoriaDto>> CreateAsync([FromBody] CategoriaDto dto, CancellationToken ct)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(dto.Nome))
					return BadRequest("Nome é obrigatório");

				// var novaCategoria = await _categoriaService.CreateAsync(dto, ct);
				dto.Id = new Random().Next(100, 999); // Simulação
				_logger.LogInformation("Categoria criada: {CategoriaId}", dto.Id);
				return CreatedAtAction(nameof(GetByIdAsync), new { id = dto.Id }, dto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erro ao criar categoria");
				return StatusCode(500, "Erro interno ao criar categoria");
			}
		}

		/// <summary>
		/// Atualiza uma categoria existente
		/// </summary>
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAsync(int id, [FromBody] CategoriaDto dto, CancellationToken ct)
		{
			try
			{
				if (id != dto.Id)
					return BadRequest("ID do corpo difere do parâmetro");

				// var atualizado = await _categoriaService.UpdateAsync(id, dto, ct);
				var atualizado = id == 1; // Simulação
				if (!atualizado)
				{
					_logger.LogWarning("Categoria não encontrada para atualizar: {CategoriaId}", id);
					return NotFound();
				}
				_logger.LogInformation("Categoria atualizada: {CategoriaId}", id);
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erro ao atualizar categoria {CategoriaId}", id);
				return StatusCode(500, "Erro interno ao atualizar categoria");
			}
		}

		/// <summary>
		/// Remove uma categoria
		/// </summary>
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(int id, CancellationToken ct)
		{
			try
			{
				// var removido = await _categoriaService.DeleteAsync(id, ct);
				var removido = id == 1; // Simulação
				if (!removido)
				{
					_logger.LogWarning("Categoria não encontrada para remover: {CategoriaId}", id);
					return NotFound();
				}
				_logger.LogInformation("Categoria removida: {CategoriaId}", id);
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erro ao remover categoria {CategoriaId}", id);
				return StatusCode(500, "Erro interno ao remover categoria");
			}
		}
	}
}
