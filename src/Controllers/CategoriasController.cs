using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic; // Adicione este using
using EmpresaX.POS.API.Services;
using EmpresaX.POS.API.Modelos.DTOs; // Adicione este using

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriasService _service;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ICategoriasService service, ILogger<CategoriasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // --- ADICIONE ESTE MÉTODO ---
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _service.GetAllAsync();
            return Ok(categorias);
        }
    }
}