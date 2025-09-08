using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PrevenTech.Controllers
{
    [Route("api")]
    [ApiController]
    public class Ping : ControllerBase
    {
        [HttpGet("ping")]
        public string ping() => "pong";
    }
}
