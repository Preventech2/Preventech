using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.Models;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;

namespace Preventech.Core.Controllers
{
    [Route("api/peca")]
    [ApiController]
    public class PecaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PecaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("cadastro")]
        public async Task<ApiResponse<Peca>> AddPeca([FromBody] Peca peca)
        {
            if (peca == null)
            {
                return new ApiResponse<Peca>
                {
                    Success = false,
                    Message = "Dados da peça inválidos",
                    Data = null
                };
            }

            try
            {
                // Adiciona a peça ao contexto
                _context.Pecas.Add(peca);

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();

                return new ApiResponse<Peca>
                {
                    Success = true,
                    Message = "Peça cadastrada com sucesso",
                    Data = peca
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Peca>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar peça: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet]
        public async Task<ApiResponse<List<Peca>>> GetPecas()
        {
            try
            {
                var pecas = await _context.Pecas.ToListAsync();
                return new ApiResponse<List<Peca>>
                {
                    Success = true,
                    Message = "Peças recuperadas com sucesso",
                    Data = pecas
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Peca>>
                {
                    Success = false,
                    Message = $"Erro ao buscar peças: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<Peca>> GetPecaById(int id)
        {
            try
            {
                var peca = await _context.Pecas.FindAsync(id);
                return new ApiResponse<Peca>
                {
                    Success = true,
                    Message = "Peça recuperada com sucesso",
                    Data = peca
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Peca>
                {
                    Success = false,
                    Message = $"Erro ao buscar peça: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPut("editar/{id}")]
        public async Task<ApiResponse<Peca>> UpdatePeca(Guid id, [FromBody] Peca pecaAtualizada)
        {
            if (id != pecaAtualizada.Id)
            {
                return new ApiResponse<Peca>
                {
                    Success = false,
                    Message = "O ID da rota não corresponde ao ID da peça fornecida",
                    Data = null
                };
            }

            try
            {
                var pecaAtual = await _context.Pecas.FindAsync(id);

                if (pecaAtual == null)
                {
                    return new ApiResponse<Peca>
                    {
                        Success = false,
                        Message = "Peça nã encontrada",
                        Data = null
                    };
                }

                _context.Entry(pecaAtual).CurrentValues.SetValues(pecaAtualizada);

                await _context.SaveChangesAsync();

                return new ApiResponse<Peca>
                {
                    Success = true,
                    Message = "Peça editada com sucesso",
                    Data = pecaAtual
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Peca>
                {
                    Success = false,
                    Message = $"Erro ao editar peça: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpDelete("excluir/{id}")]
        public async Task<ApiResponse<object>> DeletePeca(Guid id)
        {
            try
            {
                var pecaParaExcluir = await _context.Pecas.FindAsync(id);

                if (pecaParaExcluir == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Peça não encontrada"
                    };
                }

                _context.Pecas.Remove(pecaParaExcluir);

                await _context.SaveChangesAsync();

                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Peça excluída com sucesso."
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Erro ao excluir peça: {ex.Message}"
                };
            }
        }
    }
}