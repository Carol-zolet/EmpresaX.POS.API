using Microsoft.AspNetCore.Mvc;

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/v1/teste")]
    public class TesteController : ControllerBase
    {
        [HttpGet("ping")]
        public ActionResult Ping()
        {
            return Ok(new { 
                mensagem = "Sistema funcionando!", 
                data = DateTime.Now,
                versao = "1.0.0"
            });
        }

        [HttpGet("banco")]
        public ActionResult TesteBanco()
        {
            return Ok(new { 
                banco = "SQLite conectado",
                status = "OK"
            });
        }
    }
}


