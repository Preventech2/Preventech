using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PrevenTech.Controllers
{
    [Route("api")]
    [ApiController]
    public class Ping : ControllerBase
    {
        [HttpGet("ping")]
        public string a()
        {
            return "pong";
        }

        [HttpGet("ping2")]
        public string b()
        {
            return "zocca gay";
        }

        [HttpGet("/adsasda/{aaa}")]
        public string getMaquinas(string aaa)
        {
            return aaa;
        }

        [HttpPost("/cadastro/maquina")]
        public string cadastrarMaquina([FromBody] Core.Maquina maquina) 
        {
            Console.WriteLine(maquina.ToString());
            return maquina.ToString();
        }



    }
}
