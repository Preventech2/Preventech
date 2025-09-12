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
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

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
        public async Task<ActionResult<string>> LoginUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("Dados do usuário inválidos.");
            }

            try
            {
                // Procura usuario por id
                Usuario? usuarioEncontrado = await _context.Usuarios
                                .Where(user => user.Id == usuario.Id
                // Em caso de uso de criptografia, deve-se criptografar/descriptografar antes a senha
                                && user.Senha == usuario.Senha) 
                                .FirstOrDefaultAsync();

                if (usuarioEncontrado == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Credenciais inválidas.");
                }

                return Ok($"Usuário '{usuario.Nome}' logado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar usuário: {ex.Message}");
            }
        }
    }
}
