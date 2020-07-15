using Microsoft.AspNetCore.Mvc;

namespace OrigemDestino.Controllers.Api
{
    [ApiController]
    [Route("api/ping")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("pong");
        }
    }
}