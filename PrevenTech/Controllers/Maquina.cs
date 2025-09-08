using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PrevenTech.Controllers
{
    [Route("api/maquina")]
    [ApiController]
    public class Maquina : ControllerBase
    {
        [HttpGet("ping")]
        public string a() => "pong"; 

        [HttpPost("cadastro")]
        public string cadastrarMaquina([FromBody] Core.Maquina maquina)
        {
            Console.Write("Cadastrando máquina: ");
            Console.WriteLine(maquina.ToString());
            return maquina.ToString();}



    }
}
