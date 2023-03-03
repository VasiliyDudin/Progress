using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameSession.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpGet("rady")]
        public IActionResult Rady()
        {
            return Ok();
        }
    }
}
