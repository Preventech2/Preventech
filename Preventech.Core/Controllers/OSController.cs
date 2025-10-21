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
                // Adiciona as peças ao contexto
                if (ordem.Pecas != null && ordem.Pecas.Any())
                    foreach (var peca in ordem.Pecas)
                        _context.Pecas.Attach(peca);
                    
                
                // Adiciona a OS ao contexto
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
                var ordem = await _context.OrdensServico.Include(o => o.TecnicoResponsavel).ToListAsync();
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
        public async Task<ApiResponse<OrdemServico>> GetOS(Guid id)
        {
            try
            {
                var ordem = await _context.OrdensServico
                    .Include(o => o.Requisitante)
                    .Include(o => o.TecnicoResponsavel)
                    .FirstOrDefaultAsync(e => e.Id == id);

                return new ApiResponse<OrdemServico>
                {
                    Success = true,
                    Message = "Ordem de Serviço recuperados com sucesso",
                    Data = ordem
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrdemServico>
                {
                    Success = false,
                    Message = $"Erro ao buscar Ordem de Servicos: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet("/user/{id}")]
        public async Task<ApiResponse<List<OrdemServico>>> GetOSByResponsavel(int id)
        {
            try
            {
                var ordens = await _context.OrdensServico
                    .Include(o => o.Requisitante)
                    .Include(o => o.TecnicoResponsavel)
                    .Where(e => e.TecnicoResponsavelId == id)
                    .ToListAsync();

                return new ApiResponse<List<OrdemServico>>
                {
                    Success = true,
                    Message = "Ordem de Serviço recuperados com sucesso",
                    Data = ordens
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

        [HttpPut("{id}")]
        public async Task<ApiResponse<OrdemServico>> UpdateOS(Guid id, [FromBody] OrdemServico updatedOrdem)
        {
            if (updatedOrdem == null || id != updatedOrdem.Id)
            {
                return new ApiResponse<OrdemServico>
                {
                    Success = false,
                    Message = "Dados inválidos para atualização.",
                    Data = null
                };
            }

            var existingOrdem = await _context.OrdensServico
                    .Include(o => o.Requisitante)
                    .Include(o => o.TecnicoResponsavel)
                    .FirstOrDefaultAsync(e => e.Id == id);

            if (existingOrdem == null)
            {
                return new ApiResponse<OrdemServico>
                {
                    Success = false,
                    Message = "Ordem de Serviço não encontrada.",
                    Data = null
                };
            }

            try
            {
                existingOrdem.Titulo = updatedOrdem.Titulo;
                existingOrdem.Descricao = updatedOrdem.Descricao;
                existingOrdem.Observacoes = updatedOrdem.Observacoes;
                existingOrdem.Status = updatedOrdem.Status;
                existingOrdem.TecnicoResponsavelId = updatedOrdem.TecnicoResponsavelId;
                existingOrdem.RequisitanteId = updatedOrdem.RequisitanteId;
                existingOrdem.EquipamentoId = updatedOrdem.EquipamentoId;
                existingOrdem.Pecas = updatedOrdem.Pecas;

                await _context.SaveChangesAsync();

                return new ApiResponse<OrdemServico>
                {
                    Success = true,
                    Message = "Ordem de Serviço atualizada com sucesso.",
                    Data = existingOrdem
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrdemServico>
                {
                    Success = false,
                    Message = $"Erro ao atualizar Ordem de Serviço: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
