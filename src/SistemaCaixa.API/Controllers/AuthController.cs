using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCaixa.Application.Services;
using SistemaCaixa.Domain.DTOs;

namespace SistemaCaixa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public AuthController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var user = await _userService.GetByEmailAsync(req.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Senha, user.SenhaHash))
            return Unauthorized(new { message = "Usuário ou senha inválidos" });

        var token = _jwtService.GenerateToken(user);
        return Ok(new { token });
    }
}

// DTO de login
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
