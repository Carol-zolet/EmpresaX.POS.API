using Microsoft.AspNetCore.Mvc;

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VendasController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetVendas()
        {
            var vendas = new[]
            {
                new { Id = 1, Data = DateTime.Today, Cliente = "João Silva", Valor = 299.90m, Status = "Finalizada" },
                new { Id = 2, Data = DateTime.Today.AddDays(-1), Cliente = "Maria Santos", Valor = 1450.00m, Status = "Finalizada" }
            };
            
            return Ok(new { Sucesso = true, Total = vendas.Length, Vendas = vendas });
        }

        [HttpPost]
        public IActionResult CreateVenda([FromBody] dynamic vendaData)
        {
            var novoId = new Random().Next(1000, 9999);
            return Ok(new { 
                Sucesso = true, 
                Mensagem = "Venda registrada com sucesso",
                Id = novoId,
                Venda = vendaData
            });
        }
    }
}


