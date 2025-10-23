using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EmpresaX.POS.API.Services;
// Crie este DTO na sua pasta de Modelos/DTOs
public class FechamentoCaixaDto { public decimal ValorInformado { get; set; } }

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaixaController : ControllerBase
    {
        private readonly ICaixaService _service;
        private readonly ILogger<CaixaController> _logger;

        public CaixaController(ICaixaService service, ILogger<CaixaController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("{id}/fechar")]
        public async Task<IActionResult> Fechar(int id, [FromBody] FechamentoCaixaDto dto)
        {
            try
            {
                var caixaFechado = await _service.FecharCaixaAsync(id, dto.ValorInformado);
                return Ok(caixaFechado);
            }
            catch (System.Exception ex) // Tratamento de exceções genérico
            {
                // Aqui você trataria as exceções customizadas (CaixaNaoEncontrado, etc.)
                // retornando NotFound(), Conflict(), etc.
                return StatusCode(500, ex.Message);
            }
        }
    }
}


