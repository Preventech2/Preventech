using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{nome}")]
        public ApiResponse<GrupoPerfil> GetByName(string nome)
        {
            var grupoPerfil = _context.GruposPerfis.FirstOrDefault(g => g.Nome == nome);
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
                var existingGrupoPerfil = await _context.GruposPerfis.FindAsync(grupoPerfil.Nome);
                if (existingGrupoPerfil == null)
                {
                    Console.WriteLine($"Group not found: {grupoPerfil.Nome}");

                    return new ApiResponse<GrupoPerfil>
                    {
                        Success = false,
                        Message = "Grupo não encontrado",
                        Data = null
                    };
                }

                Console.WriteLine($"Editing group: {grupoPerfil.Nome} with permissions: {grupoPerfil.Permissoes}");

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
            try
            {
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
                var existingGrupoPerfil = await _context.GruposPerfis.FindAsync(grupoPerfil.Nome);
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
