using Microsoft.AspNetCore.Mvc;
using EmpresaX.POS.API.Services; // Garante que a interface do serviço seja encontrada
using Microsoft.Extensions.Logging; // Garante que o ILogger seja encontrado

[ApiController]
[Route("[controller]")]
public class ContasController : ControllerBase
{
    private readonly IContaService _service;
    private readonly ILogger<ContasController> _logger;

    // Este é o novo construtor que aceita as dependências
    public ContasController(IContaService service, ILogger<ContasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // TODO: Implementar os métodos de action (Get, Post, etc.) que usarão o _service
    // Exemplo:
    // [HttpGet]
    // public async Task<IActionResult> GetContas()
    // {
    //     var contas = await _service.GetAllAsync();
    //     return Ok(contas);
    // }
}