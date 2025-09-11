using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.Models;
using Preventech.Core.DatabaseContexts;

namespace Preventech.Core.Controllers
{
    [Route("api/equipamentos")]
    [ApiController]
    public class EquipamentoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipamentoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("cadastro")]
        public async Task<ActionResult<string>> CadastrarEquipamento([FromBody] Equipamento equipamento)
        {
            if (equipamento == null)
            {
                return BadRequest("Dados do equipamento inválidos.");
            }

            try
            {
                // Adiciona o equipamento ao contexto
                _context.Equipamentos.Add(equipamento);
                
                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();
                
                return Ok($"Equipamento '{equipamento.Nome}' cadastrado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar equipamento: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Equipamento>>> GetEquipamentos()
        {
            try
            {
                var equipamentos = await _context.Equipamentos.ToListAsync();
                return Ok(equipamentos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar equipamentos: {ex.Message}");
            }
        }

        [HttpGet("{patrimonio}")]
        public async Task<ActionResult<Equipamento>> GetEquipamento(string patrimonio)
        {
            try
            {
                var equipamento = await _context.Equipamentos
                    .FirstOrDefaultAsync(e => e.Patrimonio == patrimonio);
                
                if (equipamento == null)
                {
                    return NotFound($"Equipamento com patrimônio '{patrimonio}' não encontrado.");
                }
                
                return Ok(equipamento);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar equipamento: {ex.Message}");
            }
        }
    }
}
