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
                    .Include(o => o.Pecas)       
                        .ThenInclude(p => p.Peca)
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

        [HttpPost("pecas")]
        public async Task<ApiResponse<OrdemServicoPeca>> AddPecaOrdemServico([FromBody] OrdemServicoPeca osPeca)
        {
            if (osPeca == null || osPeca.QtdPecas <= 0)
            {
                return new ApiResponse<OrdemServicoPeca> { Success = false, Message = "Dados inválidos." };
            }

            try
            {
                var pecaEstoque = await _context.Pecas.FindAsync(osPeca.IdPeca);
                
                if (pecaEstoque == null)
                {
                    return new ApiResponse<OrdemServicoPeca> { Success = false, Message = "Peça não encontrada no estoque." };
                }

                // Verifica se tem quantidade suficiente no estoque
                if (pecaEstoque.Quantidade < osPeca.QtdPecas)
                {
                    return new ApiResponse<OrdemServicoPeca> 
                    { 
                        Success = false, 
                        Message = $"Estoque insuficiente. Disponível: {pecaEstoque.Quantidade}, Solicitado: {osPeca.QtdPecas}" 
                    };
                }

                var osExiste = await _context.OrdensServico.AnyAsync(o => o.Id == osPeca.IdOrdemServico);
                if (!osExiste)
                {
                    return new ApiResponse<OrdemServicoPeca> { Success = false, Message = "A Ordem de Serviço informada não existe." };
                }

                var relacaoExistente = await _context.OrdensServicoPecas
                    .FirstOrDefaultAsync(x => x.IdOrdemServico == osPeca.IdOrdemServico && x.IdPeca == osPeca.IdPeca);

                if (relacaoExistente != null)
                {
                    relacaoExistente.QtdPecas += osPeca.QtdPecas;
                }
                else
                {
                    _context.Add(osPeca);
                }

                // Atualiza O ESTOQUE
                pecaEstoque.Quantidade -= osPeca.QtdPecas;

                // Salva tudo numa única transação
                await _context.SaveChangesAsync();

                return new ApiResponse<OrdemServicoPeca>
                {
                    Success = true,
                    Message = "Peça adicionada e estoque atualizado!",
                    Data = osPeca
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrdemServicoPeca>
                {
                    Success = false,
                    Message = $"Erro ao processar: {ex.Message}"
                };
            }
        }

        [HttpDelete("pecas/{idOrdem}/{idPeca}")]
        public async Task<ApiResponse<bool>> RemovePeca(Guid idOrdem, Guid idPeca)
        {
            try
            {
                // Busca a relação (Saber quantas peças foram usadas)
                var relacao = await _context.OrdensServicoPecas
                    .FirstOrDefaultAsync(x => x.IdOrdemServico == idOrdem && x.IdPeca == idPeca);

                if (relacao == null)
                    return new ApiResponse<bool> { Success = false, Message = "Peça não vinculada a esta OS." };

                // Busca a peça no estoque para devolver
                var pecaEstoque = await _context.Pecas.FindAsync(idPeca);

                if (pecaEstoque != null)
                {
                    // DEVOLVE AO ESTOQUE
                    pecaEstoque.Quantidade += relacao.QtdPecas;
                }

                // Remove a relação da OS
                _context.Remove(relacao);
                
                await _context.SaveChangesAsync();
                return new ApiResponse<bool> { Success = true, Message = "Peça removida e devolvida ao estoque." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { Success = false, Message = $"Erro: {ex.Message}" };
            }
        }
    }
}
