using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameSession.API
{
    //[Route("api/[controller]")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [HttpGet]
        public IActionResult Rady()
        {
            return Ok();
        }
    }
}
