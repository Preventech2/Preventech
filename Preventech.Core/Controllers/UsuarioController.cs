using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
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
                // verifica se usuario com este cpf já existe
                var usuarioExistente = await _context.Usuarios
                    .Where(u => u.Cpf == usuario.Cpf)
                    .FirstOrDefaultAsync();

                if (usuarioExistente != null)
                {
                    return new ApiResponse<Usuario>
                    {
                        Success = false,
                        Message = "Você já está cadastrado.",
                        Data = usuarioExistente
                    };
                }

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
        [AllowAnonymous]
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
                                .Include(u => u.Grupo) // Inclui o grupo na consulta
                                .Where(user => user.Cpf == usuario.Cpf
                                // Em caso de uso de criptografia, deve-se criptografar/descriptografar antes a senha
                                && user.Senha == usuario.Senha)
                                .FirstOrDefaultAsync();

                if (usuarioEncontrado == null)
                {
                    return new ApiResponse<Usuario>
                    {
                        Success = false,
                        Message = "CPF ou senha inválidos.",
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
                var usuarios = await _context.Usuarios
                    .Include(u => u.Grupo) // Inclui o grupo na consulta
                    .Select(u => new Usuario
                    {
                        Id = u.Id,
                        Cpf = u.Cpf,
                        Nome = u.Nome,
                        Grupo = u.Grupo
                    })
                    .ToListAsync();

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

        [HttpPost]
        public async Task<ApiResponse<Usuario>> GetUsuarioByCpf([FromBody] Usuario usuario)
        {
            if (usuario == null || string.IsNullOrWhiteSpace(usuario.Cpf))
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
                var usuarioEncontrado = await _context.Usuarios
                    .Include(u => u.Grupo) // Inclui o grupo na consulta
                    .Where(u => u.Cpf == usuario.Cpf)
                    .Select(u => new Usuario
                    {
                        Id = u.Id,
                        Cpf = u.Cpf,
                        Nome = u.Nome,
                        Email = u.Email,
                        Grupo = u.Grupo
                    })
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
                    Message = "Usuário encontrado com sucesso.",
                    Data = usuarioEncontrado
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Usuario>
                {
                    Success = false,
                    Message = $"Erro ao buscar usuário: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPost("add-grupo")]
        public async Task<ApiResponse<Usuario>> AddGrupoInUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null || string.IsNullOrWhiteSpace(usuario.Cpf))
            {
                return new ApiResponse<Usuario>
                {
                    Success = false,
                    Message = "CPF do usuário é obrigatório",
                    Data = null
                };
            }

            try
            {
                // Verifica se o usuário existe e carrega o grupo atual
                var usuarioExistente = await _context.Usuarios
                    .Include(u => u.Grupo) // Carrega o grupo atual
                    .Where(u => u.Cpf == usuario.Cpf)
                    .FirstOrDefaultAsync();

                if (usuarioExistente == null)
                {
                    return new ApiResponse<Usuario>
                    {
                        Success = false,
                        Message = "Usuário não encontrado.",
                        Data = null
                    };
                }

                // Se nenhum grupo foi fornecido ou o grupo é vazio, remove a associação
                if (usuario.Grupo == null || string.IsNullOrWhiteSpace(usuario.Grupo.Nome))
                {
                    usuarioExistente.Grupo = null;
                }
                else
                {
                    // Verifica se o grupo existe no banco
                    var novoGrupo = await _context.GruposPerfis
                        .Where(g => g.Nome == usuario.Grupo.Nome)
                        .FirstOrDefaultAsync();

                    // Se o grupo fornecido não existe, retorna erro
                    if (novoGrupo == null)
                    {
                        return new ApiResponse<Usuario>
                        {
                            Success = false,
                            Message = "Grupo não encontrado.",
                            Data = null
                        };
                    }

                    // Atualiza a associação do grupo
                    usuarioExistente.Grupo = novoGrupo;
                }

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();

                return new ApiResponse<Usuario>
                {
                    Success = true,
                    Message = usuarioExistente.Grupo != null ?
                        $"Usuário associado ao grupo '{usuarioExistente.Grupo.Nome}' com sucesso." :
                        "Usuário removido do grupo com sucesso.",
                    Data = usuarioExistente
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Usuario>
                {
                    Success = false,
                    Message = $"Erro ao modificar grupo do usuário: {ex.Message}",
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
                // Se um grupo foi fornecido, busca o grupo existente
                if (usuario.Grupo != null && !string.IsNullOrWhiteSpace(usuario.Grupo.Nome))
                {
                    var grupoExistente = await _context.GruposPerfis
                        .Where(g => g.Nome == usuario.Grupo.Nome)
                        .FirstOrDefaultAsync();

                    if (grupoExistente != null)
                    {
                        usuario.Grupo = grupoExistente;
                        _context.Entry(grupoExistente).State = EntityState.Unchanged;
                    }
                    else
                    {
                        usuario.Grupo = null; // Remove referência inválida
                    }
                }

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

        [HttpPost("editar")]
        public async Task<ApiResponse<Usuario>> UpdateUsuarioAsync([FromBody] Usuario usuario)
        {
            Console.WriteLine("Entrando na controller");
            if (usuario == null
                || string.IsNullOrWhiteSpace(usuario.Cpf)
                || string.IsNullOrWhiteSpace(usuario.Nome)
                || string.IsNullOrWhiteSpace(usuario.Email))
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
                var usuarioExistente = await _context.Usuarios
                    .Include(u => u.Grupo) // Inclui o grupo na consulta
                    .Where(u => u.Cpf == usuario.Cpf)
                    .FirstOrDefaultAsync();

                if (usuarioExistente == null)
                {
                    return new ApiResponse<Usuario>
                    {
                        Success = false,
                        Message = "Usuário não encontrado",
                        Data = null
                    };
                }

                // Atualiza os campos do usuário existente
                usuarioExistente.Nome = usuario.Nome;
                usuarioExistente.Email = usuario.Email;
                // if (!string.IsNullOrWhiteSpace(usuario.Senha))
                // {
                //     usuarioExistente.Senha = usuario.Senha; // Atualiza a senha somente se fornecida
                // }

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();

                return new ApiResponse<Usuario>
                {
                    Success = true,
                    Message = "Usuário atualizado com sucesso",
                    Data = usuarioExistente
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Usuario>
                {
                    Success = false,
                    Message = $"Erro ao atualizar usuário: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
