using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;
using Preventech.Core.Models;

namespace Preventech.Core.Controllers
{
    [Route("api/grupos-perfis")]
    [ApiController]
    public class GrupoPerfilController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public ApiResponse<List<GrupoPerfil>> GetAll()
        {
            var gruposPerfis = _context.GruposPerfis.ToList();
            return new ApiResponse<List<GrupoPerfil>>
            {
                Success = true,
                Message = "Grupos recuperados com sucesso",
                Data = gruposPerfis
            };
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<GrupoPerfil>> GetById(int id)
        {
            var grupoPerfil = await _context.GruposPerfis.FindAsync(id);
            if (grupoPerfil == null)
            {
                return new ApiResponse<GrupoPerfil>
                {
                    Success = false,
                    Message = "Grupo não encontrado",
                    Data = null
                };
            }
            
            return new ApiResponse<GrupoPerfil>
            {
                Success = true,
                Message = "Grupo recuperado com sucesso",
                Data = grupoPerfil
            };
        }

        [HttpPost("editar")]
        public async Task<ApiResponse<GrupoPerfil>> EditGrupoPerfil([FromBody] GrupoPerfil grupoPerfil)
        {
            try
            {
                // Busca o grupo existente pelo ID
                var existingGrupoPerfil = await _context.GruposPerfis.FindAsync(grupoPerfil.Id);
                if (existingGrupoPerfil == null)
                {
                    return new ApiResponse<GrupoPerfil>
                    {
                        Success = false,
                        Message = "Grupo não encontrado",
                        Data = null
                    };
                }

                // Verifica se o novo nome já existe em outro grupo (diferente do atual)
                var nomeExiste = await _context.GruposPerfis
                    .AnyAsync(g => g.Nome == grupoPerfil.Nome && g.Id != grupoPerfil.Id);
                
                if (nomeExiste)
                {
                    return new ApiResponse<GrupoPerfil>
                    {
                        Success = false,
                        Message = $"Já existe um grupo com o nome '{grupoPerfil.Nome}'",
                        Data = null
                    };
                }

                existingGrupoPerfil.Nome = grupoPerfil.Nome;
                existingGrupoPerfil.Permissoes = grupoPerfil.Permissoes;

                _context.GruposPerfis.Update(existingGrupoPerfil);
                await _context.SaveChangesAsync();

                return new ApiResponse<GrupoPerfil>
                {
                    Success = true,
                    Message = "Grupo editado com sucesso",
                    Data = existingGrupoPerfil
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GrupoPerfil>
                {
                    Success = false,
                    Message = $"Erro ao editar grupo: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPost("cadastro")]
        public async Task<ApiResponse<GrupoPerfil>> AddGrupoPerfil([FromBody] GrupoPerfil grupoPerfil)
        {
            if (string.IsNullOrWhiteSpace(grupoPerfil.Nome))
            {
                return new ApiResponse<GrupoPerfil>
                {
                    Success = false,
                    Message = "Nome do grupo é obrigatório",
                    Data = null
                };
            }

            try
            {
                // Verifica se o novo nome já existe em outro grupo (diferente do atual)
                var nomeExiste = await _context.GruposPerfis
                    .AnyAsync(g => g.Nome == grupoPerfil.Nome);
                
                if (nomeExiste)
                {
                    return new ApiResponse<GrupoPerfil>
                    {
                        Success = false,
                        Message = $"Já existe um grupo com o nome '{grupoPerfil.Nome}'",
                        Data = null
                    };
                }

                _context.GruposPerfis.Add(grupoPerfil);
                await _context.SaveChangesAsync();

                return new ApiResponse<GrupoPerfil>
                {
                    Success = true,
                    Message = "Grupo cadastrado com sucesso",
                    Data = grupoPerfil
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GrupoPerfil>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar grupo: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPost("deletar")]
        public async Task<ApiResponse<GrupoPerfil>> DeleteGrupoPerfil([FromBody] GrupoPerfil grupoPerfil)
        {
            try
            {
                var existingGrupoPerfil = await _context.GruposPerfis
                    .FirstOrDefaultAsync(g => g.Id == grupoPerfil.Id);
                if (existingGrupoPerfil == null)
                {
                    return new ApiResponse<GrupoPerfil>
                    {
                        Success = false,
                        Message = "Grupo não encontrado",
                        Data = null
                    };
                }

                _context.GruposPerfis.Remove(existingGrupoPerfil);
                await _context.SaveChangesAsync();

                return new ApiResponse<GrupoPerfil>
                {
                    Success = true,
                    Message = "Grupo deletado com sucesso",
                    Data = existingGrupoPerfil
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GrupoPerfil>
                {
                    Success = false,
                    Message = $"Erro ao deletar grupo: {ex.Message}",
                    Data = null
                };
            }
        }
    }

}
