using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsuariosController : ControllerBase
    {
        [HttpGet]
        [Authorize] // Reativar autenticação
        public IActionResult GetUsuarios()
        {
            var usuarios = new[]
            {
                new { Id = 1, Nome = "Administrador", Email = "admin@empresax.com", Perfil = "Admin", Ativo = true },
                new { Id = 2, Nome = "João Silva", Email = "joao@empresax.com", Perfil = "Vendedor", Ativo = true },
                new { Id = 3, Nome = "Maria Santos", Email = "maria@empresax.com", Perfil = "Operador", Ativo = false },
                new { Id = 4, Nome = "Pedro Costa", Email = "pedro@empresax.com", Perfil = "Gerente", Ativo = true }
            };
            
            return Ok(new { 
                Sucesso = true, 
                Total = usuarios.Length,
                Usuarios = usuarios,
                Timestamp = DateTime.Now
            });
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetUsuario(int id)
        {
            if (id <= 0)
                return BadRequest(new { Sucesso = false, Erro = "ID inválido" });

            var usuario = new { Id = id, Nome = $"Usuário {id}", Email = $"user{id}@empresax.com", Perfil = "Usuario", Ativo = true };
            return Ok(new { Sucesso = true, Usuario = usuario });
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateUsuario([FromBody] dynamic userData)
        {
            var novoId = new Random().Next(1000, 9999);
            return Ok(new { 
                Sucesso = true, 
                Mensagem = "Usuário criado com sucesso",
                Id = novoId,
                Usuario = userData,
                Timestamp = DateTime.Now
            });
        }
    }
}


