using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCaixa.Application.DTOs;
using SistemaCaixa.Application.Services;

namespace SistemaCaixa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ImportacaoController : ControllerBase
{
    private readonly IImportacaoExcelService _importacaoService;
    private readonly ILogger<ImportacaoController> _logger;

    public ImportacaoController(
        IImportacaoExcelService importacaoService,
        ILogger<ImportacaoController> logger)
    {
        _importacaoService = importacaoService;
        _logger = logger;
    }

    [HttpPost("planilha-caixa")]
    [RequestSizeLimit(10_485_760)]
    [ProducesResponseType(typeof(ImportacaoExcelResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportarPlanilhaCaixa(
        IFormFile arquivo,
        CancellationToken cancellationToken)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Arquivo Inválido",
                Detail = "Nenhum arquivo foi enviado"
            });
        }
        var extensoesPermitidas = new[] { ".xlsx", ".xls" };
        var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
        if (!extensoesPermitidas.Contains(extensao))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Formato Inválido",
                Detail = $"Apenas arquivos Excel são permitidos ({string.Join(", ", extensoesPermitidas)})"
            });
        }
        if (arquivo.Length > 10_485_760)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Arquivo Muito Grande",
                Detail = "O arquivo não pode exceder 10MB"
            });
        }
        try
        {
            _logger.LogInformation(
                "Recebendo arquivo para importação: {FileName} ({Size} bytes)",
                arquivo.FileName,
                arquivo.Length
            );
            using var stream = arquivo.OpenReadStream();
            var resultado = await _importacaoService.ImportarPlanilhaAsync(
                stream,
                arquivo.FileName,
                User.Identity?.Name ?? "Sistema",
                cancellationToken
            );
            if (resultado.Sucesso)
            {
                _logger.LogInformation(
                    "Importação concluída com sucesso: CaixaId={CaixaId}, Movimentos={Movimentos}",
                    resultado.CaixaId,
                    resultado.MovimentosImportados
                );
                return Ok(resultado);
            }
            else
            {
                _logger.LogWarning(
                    "Importação concluída com erros: {Erros} erros encontrados",
                    resultado.ErrosEncontrados
                );
                return Ok(resultado);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar importação");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Erro ao Importar",
                Detail = "Ocorreu um erro ao processar a planilha. Verifique o formato e tente novamente."
            });
        }
    }
}
