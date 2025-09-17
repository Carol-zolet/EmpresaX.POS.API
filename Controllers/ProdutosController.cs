using Microsoft.AspNetCore.Mvc;
using EmpresaX.POS.API.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks; // Necessário para Task

[ApiController]
[Route("[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutosService _service;
    private readonly ILogger<ProdutosController> _logger;

    // 1. Construtor corrigido para aceitar as dependências
    public ProdutosController(IProdutosService service, ILogger<ProdutosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // 2. Método GetById que o teste precisa para compilar
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var produto = await _service.GetByIdAsync(id);
        if (produto == null)
        {
            return NotFound();
        }
        return Ok(produto);
    }
}