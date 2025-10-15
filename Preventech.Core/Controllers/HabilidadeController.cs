using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;
using Preventech.Core.Models;

namespace Preventech.Core.Controllers
{
    [Route("api/habilidades")]
    [ApiController]
    public class HabilidadeController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public ApiResponse<List<Habilidade>> GetAll()
        {
            var habilidades = _context.Habilidades.Include(h => h.Categoria).ToList();
            return new ApiResponse<List<Habilidade>>
            {
                Success = true,
                Message = "Habilidades recuperadas com sucesso",
                Data = habilidades
            };
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<Habilidade>> GetById(int id)
        {
            var habilidade = await _context.Habilidades
                .Include(h => h.Categoria) // Inclui a navegação para Categoria
                .FirstOrDefaultAsync(h => h.Id == id);

            if (habilidade == null)
            {
                return new ApiResponse<Habilidade>
                {
                    Success = false,
                    Message = "Habilidade não encontrada",
                    Data = null
                };
            }

            return new ApiResponse<Habilidade>
            {
                Success = true,
                Message = "Habilidade recuperada com sucesso",
                Data = habilidade
            };
        }

        [HttpPost]
        public async Task<ApiResponse<List<Habilidade>>> GetByUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (usuario == null || string.IsNullOrWhiteSpace(usuario.Cpf))
                {
                    return new ApiResponse<List<Habilidade>>
                    {
                        Success = false,
                        Message = "Dados do usuário inválidos",
                        Data = null
                    };
                }

                var habilidades = await _context.Habilidades
                    .Include(h => h.Categoria) // Inclui a navegação para Categoria
                    .Where(h => h.Usuario!.Cpf == usuario.Cpf)
                    .ToListAsync();

                if (habilidades == null)
                {
                    return new ApiResponse<List<Habilidade>>
                    {
                        Success = false,
                        Message = "Habilidade não encontrada",
                        Data = null
                    };
                }

                return new ApiResponse<List<Habilidade>>
                {
                    Success = true,
                    Message = "Habilidade recuperada com sucesso",
                    Data = habilidades
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Habilidade>>
                {
                    Success = false,
                    Message = $"Erro ao buscar habilidade: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPost("cadastro")]
        public async Task<ApiResponse<Habilidade>> AddHabilidade([FromBody] Habilidade habilidade)
        {
            if (habilidade == null
                || habilidade.Categoria == null
                || habilidade.Usuario == null
                || string.IsNullOrWhiteSpace(habilidade.Usuario.Cpf)
                || habilidade.Categoria.Id <= 0
                || string.IsNullOrWhiteSpace(habilidade.Descricao))
            {
                return new ApiResponse<Habilidade>
                {
                    Success = false,
                    Message = "Categoria, usuário e descrição são obrigatórios",
                    Data = null
                };
            }

            try
            {
                // Buscando a categoria pelo ID para garantir que ela existe
                var categoria = await _context.HabilidadesSistema.FindAsync(habilidade.Categoria.Id);
                if (categoria == null)
                {
                    return new ApiResponse<Habilidade>
                    {
                        Success = false,
                        Message = "Categoria não encontrada",
                        Data = null
                    };
                }

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Cpf == habilidade.Usuario.Cpf);
                if (usuario == null)
                {
                    return new ApiResponse<Habilidade>
                    {
                        Success = false,
                        Message = "Usuário não encontrado",
                        Data = null
                    };
                }

                habilidade.Categoria = categoria;
                habilidade.Usuario = usuario;
                _context.Habilidades.Add(habilidade);
                await _context.SaveChangesAsync();

                return new ApiResponse<Habilidade>
                {
                    Success = true,
                    Message = "Habilidade cadastrada com sucesso",
                    Data = habilidade
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Habilidade>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar habilidade: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPost("editar")]
        public async Task<ApiResponse<Habilidade>> EditHabilidade([FromBody] Habilidade habilidade)
        {
            if (habilidade.Categoria == null 
                || string.IsNullOrWhiteSpace(habilidade.Categoria.Nome)
                || string.IsNullOrWhiteSpace(habilidade.Descricao))
            {
                return new ApiResponse<Habilidade>
                {
                    Success = false,
                    Message = "A categoria é obrigatória",
                    Data = null
                };
            }

            try
            {
                // Busca a habilidade existente pelo ID
                var habilidadeEncontrada = await _context.Habilidades.FindAsync(habilidade.Id);
                if (habilidadeEncontrada == null)
                {
                    return new ApiResponse<Habilidade>
                    {
                        Success = false,
                        Message = "Habilidade não encontrada",
                        Data = null
                    };
                }

                // Buscando a nova categoria pelo ID para garantir que ela existe
                var novaCategoria = await _context.HabilidadesSistema.FindAsync(habilidade.Categoria.Id);
                if (novaCategoria == null)
                {
                    return new ApiResponse<Habilidade>
                    {
                        Success = false,
                        Message = "Categoria não encontrada",
                        Data = null
                    };
                }

                habilidadeEncontrada.Categoria = novaCategoria;
                habilidadeEncontrada.Descricao = habilidade.Descricao;

                _context.Habilidades.Update(habilidadeEncontrada);
                await _context.SaveChangesAsync();

                return new ApiResponse<Habilidade>
                {
                    Success = true,
                    Message = "Habilidade editada com sucesso",
                    Data = habilidadeEncontrada
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Habilidade>
                {
                    Success = false,
                    Message = $"Erro ao editar habilidade: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPost("deletar")]
        public async Task<ApiResponse<Habilidade>> DeleteHabilidade([FromBody] Habilidade habilidade)
        {
            try
            {
                var habilidadeEncontrada = await _context.Habilidades.FindAsync(habilidade.Id);
                if (habilidadeEncontrada == null)
                {
                    return new ApiResponse<Habilidade>
                    {
                        Success = false,
                        Message = "Habilidade não encontrada",
                        Data = null
                    };
                }

                _context.Habilidades.Remove(habilidadeEncontrada);
                await _context.SaveChangesAsync();

                return new ApiResponse<Habilidade>
                {
                    Success = true,
                    Message = "Habilidade deletada com sucesso",
                    Data = habilidadeEncontrada
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Habilidade>
                {
                    Success = false,
                    Message = $"Erro ao deletar habilidade: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
