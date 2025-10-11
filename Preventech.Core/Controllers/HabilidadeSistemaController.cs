using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;
using Preventech.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Preventech.Core.Controllers
{
    [Route("api/habilidades-sistema")]
    [ApiController]
    public class HabilidadeSistemaController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public ApiResponse<List<HabilidadeSistema>> GetAll()
        {
            var habilidades = _context.HabilidadesSistema.ToList();
            return new ApiResponse<List<HabilidadeSistema>>
            {
                Success = true,
                Message = "Habilidades recuperadas com sucesso",
                Data = habilidades
            };
        }

        [HttpPost("cadastro")]
        public async Task<ApiResponse<HabilidadeSistema>> AddHabilidadeSistema([FromBody] HabilidadeSistema habilidade)
        {
            if (string.IsNullOrWhiteSpace(habilidade.Nome))
            {
                return new ApiResponse<HabilidadeSistema>
                {
                    Success = false,
                    Message = "Nome da habilidade é obrigatória",
                    Data = null
                };
            }

            try
            {
                // Verifica se o novo nome já existe em outra habilidade (diferente da atual)
                var nomeExiste = await _context.HabilidadesSistema
                    .AnyAsync(h => h.Nome == habilidade.Nome);

                if (nomeExiste)
                {
                    return new ApiResponse<HabilidadeSistema>
                    {
                        Success = false,
                        Message = $"Já existe uma habilidade com o nome '{habilidade.Nome}'",
                        Data = null
                    };
                }

                _context.HabilidadesSistema.Add(habilidade);
                await _context.SaveChangesAsync();

                return new ApiResponse<HabilidadeSistema>
                {
                    Success = true,
                    Message = "Habilidade cadastrada com sucesso",
                    Data = habilidade
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HabilidadeSistema>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar habilidade: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPost("editar")]
        public async Task<ApiResponse<HabilidadeSistema>> EditHabilidadeSistema([FromBody] HabilidadeSistema habilidade)
        {
            try
            {
                // Busca a habilidade existente pelo ID
                var existingHabilidade = await _context.HabilidadesSistema.FindAsync(habilidade.Id);
                if (existingHabilidade == null)
                {
                    return new ApiResponse<HabilidadeSistema>
                    {
                        Success = false,
                        Message = "Habilidade não encontrada",
                        Data = null
                    };
                }

                // Verifica se o novo nome já existe em outra habilidade (diferente da atual)
                var nomeExiste = await _context.HabilidadesSistema
                    .AnyAsync(h => h.Nome == habilidade.Nome && h.Id != habilidade.Id);

                if (nomeExiste)
                {
                    return new ApiResponse<HabilidadeSistema>
                    {
                        Success = false,
                        Message = $"Já existe uma habilidade com o nome '{habilidade.Nome}'",
                        Data = null
                    };
                }

                existingHabilidade.Nome = habilidade.Nome;

                _context.HabilidadesSistema.Update(existingHabilidade);
                await _context.SaveChangesAsync();

                return new ApiResponse<HabilidadeSistema>
                {
                    Success = true,
                    Message = "Habilidade editada com sucesso",
                    Data = existingHabilidade
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HabilidadeSistema>
                {
                    Success = false,
                    Message = $"Erro ao editar habilidade: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPost("deletar")]
        public async Task<ApiResponse<HabilidadeSistema>> DeleteHabilidadeSistema([FromBody] HabilidadeSistema habilidade)
        {
            try
            {
                var habilidadeEncontrada = await _context.HabilidadesSistema.FindAsync(habilidade.Id);
                if (habilidadeEncontrada == null)
                {
                    return new ApiResponse<HabilidadeSistema>
                    {
                        Success = false,
                        Message = "Habilidade não encontrada",
                        Data = null
                    };
                }

                _context.HabilidadesSistema.Remove(habilidadeEncontrada);
                await _context.SaveChangesAsync();

                return new ApiResponse<HabilidadeSistema>
                {
                    Success = true,
                    Message = "Habilidade deletada com sucesso",
                    Data = habilidadeEncontrada
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HabilidadeSistema>
                {
                    Success = false,
                    Message = $"Erro ao deletar habilidade: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
