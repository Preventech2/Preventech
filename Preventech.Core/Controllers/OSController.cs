using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.Models;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;

namespace Preventech.Core.Controllers
{
    [Route("api/ordem-servico")]
    [ApiController]
    public class OSController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OSController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("cadastro")]
        public async Task<ApiResponse<OrdemServico>> CadastrarOS([FromBody] OrdemServico ordem)
        {
            if (ordem == null)
            {
                return new ApiResponse<OrdemServico>
                {
                    Success = false,
                    Message = "Dados da OS inválidos",
                    Data = null
                };
            }

            try
            {
                // Adiciona o equipamento ao contexto
                _context.OrdensServico.Add(ordem);
                
                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();
                
                return new ApiResponse<OrdemServico>
                {
                    Success = true,
                    Message = "Ordem de Serviço cadastrada com sucesso!",
                    Data = ordem
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrdemServico>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar Ordem de Serviço: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet]
        public async Task<ApiResponse<List<OrdemServico>>> GetOSs()
        {
            try
            {
                var ordem = await _context.OrdensServico.ToListAsync();
                return new ApiResponse<List<OrdemServico>>
                {
                    Success = true,
                    Message = "Ordens de Serviço recuperados com sucesso",
                    Data = ordem
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrdemServico>>
                {
                    Success = false,
                    Message = $"Erro ao buscar Ordem de Servicos: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrdemServico>> GetOS(Guid id)
        {
            try
            {
                var ordem = await _context.OrdensServico
                    .FirstOrDefaultAsync(e => e.Id == id);
                
                if (ordem == null)
                {
                    return NotFound($"Ordem de serviço com id '{id}' não encontrada.");
                }
                
                return Ok(ordem);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar ordem de serviço: {ex.Message}");
            }
        }
    }
}
