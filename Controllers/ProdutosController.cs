using Microsoft.AspNetCore.Mvc;

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProdutosController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProdutos()
        {
            var produtos = new[]
            {
                new { Id = 1, Nome = "Notebook Dell", Preco = 2500.00m, Categoria = "Informática", Estoque = 5 },
                new { Id = 2, Nome = "Mouse Logitech", Preco = 45.90m, Categoria = "Periféricos", Estoque = 20 },
                new { Id = 3, Nome = "Teclado Mecânico", Preco = 189.99m, Categoria = "Periféricos", Estoque = 8 },
                new { Id = 4, Nome = "Monitor 24\"", Preco = 899.00m, Categoria = "Monitores", Estoque = 3 }
            };
            
            return Ok(new { 
                Sucesso = true, 
                Total = produtos.Length,
                Produtos = produtos,
                Timestamp = DateTime.Now
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetProduto(int id)
        {
            if (id <= 0)
                return BadRequest(new { Sucesso = false, Erro = "ID inválido" });

            var produto = new { Id = id, Nome = $"Produto {id}", Preco = 99.99m, Categoria = "Geral", Estoque = 10 };
            return Ok(new { Sucesso = true, Produto = produto });
        }

        [HttpPost]
        public IActionResult CreateProduto([FromBody] dynamic produtoData)
        {
            var novoId = new Random().Next(1000, 9999);
            return Ok(new { 
                Sucesso = true, 
                Mensagem = "Produto criado com sucesso",
                Id = novoId,
                Produto = produtoData,
                Timestamp = DateTime.Now
            });
        }
    }
}
