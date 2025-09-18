using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmpresaX.POS.API.Services;
using EmpresaX.POS.API.Modelos.DTOs;

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _service;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IProdutosService service, ILogger<ProdutosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = await _service.GetByIdAsync(id);
            if (produto == null) { return NotFound(); }
            return Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProdutoDto produtoDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var produtoCriado = await _service.CreateAsync(produtoDto);
            return CreatedAtAction(nameof(GetById), new { id = produtoCriado.Id }, produtoCriado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProdutoDto produtoDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            try
            {
                await _service.UpdateAsync(id, produtoDto);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        // --- MÉTODO DELETE ADICIONADO ---
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent(); // Sucesso
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Recurso não encontrado
            }
        }
    }
}