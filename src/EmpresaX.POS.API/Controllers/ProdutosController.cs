using EmpresaX.POS.API.DTOs;
using EmpresaX.POS.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _repository;

        public ProdutosController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var produtos = await _repository.GetAllAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = await _repository.GetByIdAsync(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProdutoRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var produto = await _repository.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProdutoRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var produto = await _repository.UpdateAsync(id, request);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}