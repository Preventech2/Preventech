using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.Models;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;

namespace Preventech.Core.Controllers
{
    [Route("api/localizacao")]
    [ApiController]
    public class LocalizacaoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocalizacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("cadastro")]
        public async Task<ApiResponse<Equipamento>> CadastrarEquipamento([FromBody] Equipamento equipamento)
        {

            try
            {
                // Adiciona o equipamento ao contexto
                _context.Equipamentos.Add(equipamento);

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();

                return new ApiResponse<Equipamento>
                {
                    Success = true,
                    Message = "Equipamento cadastrado com sucesso",
                    Data = equipamento
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Equipamento>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar equipamento: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet]
        public async Task<ApiResponse<List<Localizacao>>> GetEquipamentos()
        {
            try
            {
                return new ApiResponse<List<Localizacao>>
                {
                    Success = true,
                    Message = "Equipamentos recuperados com sucesso",
                    Data = await _context.Localizacoes.ToListAsync()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Localizacao>>
                {
                    Success = false,
                    Message = $"Erro ao buscar equipamentos: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet("{campus}")]
        public async Task<ActionResult<Localizacao>> GetDentroCampus(int campus)
        {
            try
            {
                var localizacoes = await _context.Localizacoes
                    .Where(loc => loc.Campus == campus)
                    .ToListAsync();

                if (localizacoes == null)
                {
                    return NotFound($"campus c{campus} não encontrado.");
                }

                return Ok(localizacoes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar equipamento: {ex.Message}");
            }
        }
    }
}
