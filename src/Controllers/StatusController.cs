using Microsoft.AspNetCore.Mvc;

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "OK", timestamp = DateTime.Now });
        }
    }
}


