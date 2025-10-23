using Microsoft.AspNetCore.Mvc;
using EmpresaX.POS.API.Services;
using EmpresaX.POS.API.Modelos.DTOs;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/v1/[controller]")]
public class ContasController : ControllerBase
{
    private readonly IContaService _service;
    private readonly ILogger<ContasController> _logger;

    public ContasController(IContaService service, ILogger<ContasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var contas = await _service.GetAllAsync();
        return Ok(contas);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var conta = await _service.GetByIdAsync(id);
        if (conta == null) return NotFound();
        return Ok(conta);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContaDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var contaCriada = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = contaCriada.Id }, contaCriada);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ContaDto dto)
    {
        try
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
