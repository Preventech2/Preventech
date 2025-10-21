using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;
using Preventech.Core.Models;

namespace Preventech.Core.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpPost("cadastro")]
        public async Task<ApiResponse<Usuario>> CadastroUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return new ApiResponse<Usuario>
                {
                    Success = false,
                    Message = "Dados do usuário inválidos",
                    Data = null
                };
            }

            try
            {
                // Adiciona o usuario ao contexto
                _context.Usuarios.Add(usuario);

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();

                return new ApiResponse<Usuario>
                {
                    Success = true,
                    Message = "Usuário cadastrado com sucesso",
                    Data = usuario
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Usuario>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar usuário: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPost("login")]
        public async Task<ApiResponse<Usuario>> LoginUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return new ApiResponse<Usuario>
                {
                    Success = false,
                    Message = "Dados do usuário inválidos.",
                    Data = null
                };
            }

            try
            {
                // Procura usuario por cpf
                Usuario? usuarioEncontrado = await _context.Usuarios
                                .Where(user => user.Cpf == usuario.Cpf
                                // Em caso de uso de criptografia, deve-se criptografar/descriptografar antes a senha
                                && user.Senha == usuario.Senha)
                                .FirstOrDefaultAsync();

                if (usuarioEncontrado == null)
                {
                    return new ApiResponse<Usuario>
                    {
                        Success = false,
                        Message = "Usuário não encontrado.",
                        Data = null
                    };
                }

                return new ApiResponse<Usuario>
                {
                    Success = true,
                    Message = $"Usuário '{usuarioEncontrado.Nome}' logado com sucesso.",
                    Data = usuarioEncontrado
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Usuario>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar usuário: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet]
        public async Task<ApiResponse<List<Usuario>>> GetUsuarios()
        {
            try
            {
                var usuarios = await _context.Usuarios.ToListAsync();
                return new ApiResponse<List<Usuario>>
                {
                    Success = true,
                    Message = "Usuários buscados com sucesso.",
                    Data = usuarios
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Usuario>>
                {
                    Success = false,
                    Message = $"Erro ao buscar usuários: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPost("invite")]
        public async Task<ApiResponse<Usuario>> InviteUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return new ApiResponse<Usuario>
                {
                    Success = false,
                    Message = "Dados do usuário inválidos",
                    Data = null
                };
            }

            try
            {
                // Adiciona o usuario ao contexto
                _context.Usuarios.Add(usuario);

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();

                return new ApiResponse<Usuario>
                {
                    Success = true,
                    Message = "Usuário convidado com sucesso",
                    Data = usuario
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Usuario>
                {
                    Success = false,
                    Message = $"Erro ao convidar usuário: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
