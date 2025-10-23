using Microsoft.AspNetCore.Mvc;
using EmpresaX.POS.API.Services;

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportacaoController : ControllerBase
    {
        private readonly IImportacaoService _importacaoService;

        public ImportacaoController(IImportacaoService importacaoService)
        {
            _importacaoService = importacaoService;
        }

        [HttpPost("conciliacao-bancaria")]
        public async Task<IActionResult> UploadExtratoBancario(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhum arquivo enviado.");
            }

            // Delega o processamento do arquivo para a camada de serviço
            await _importacaoService.ProcessarExtratoBancario(file.OpenReadStream());

            return Ok(new { message = "Extrato bancário processado com sucesso." });
        }
    }
}

