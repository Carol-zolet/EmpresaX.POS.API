using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaCaixa.Application.UseCases.AbrirCaixa;
using SistemaCaixa.Application.UseCases.FecharCaixa;

namespace SistemaCaixa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaixaController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CaixaController> _logger;

    public CaixaController(IMediator mediator, ILogger<CaixaController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("abrir")]
    public async Task<IActionResult> AbrirCaixa([FromBody] AbrirCaixaCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        if (result.IsSuccess) return Ok(result.Data);
        _logger.LogWarning("Falha ao abrir caixa: {Erro}", result.Error);
        return BadRequest(result.Error);
    }

    [HttpPost("fechar")]
    public async Task<IActionResult> FecharCaixa([FromBody] FecharCaixaCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        if (result.IsSuccess) return Ok(result.Data);
        _logger.LogWarning("Falha ao fechar caixa: {Erro}", result.Error);
        return BadRequest(result.Error);
    }
}
