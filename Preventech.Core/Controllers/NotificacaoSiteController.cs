using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;
using Preventech.Core.Models;

namespace Preventech.Core.Controllers
{
    [Route("api/notificacoes")]
    [ApiController]
    public class NotificacaoSiteController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpPost]
        public ApiResponse<List<NotificacaoSiteUsuario>> GetNotificacoes([FromBody] Usuario usuario)
        {
            var notificacoes = _context.NotificacoesSiteUsuarios
                .Where(nsu => nsu.UsuarioId == usuario.Id)
                .Include(nsu => nsu.Notificacao);

            return new ApiResponse<List<NotificacaoSiteUsuario>>
            {
                Success = true,
                Message = "Notificações recuperadas com sucesso",
                Data = [.. notificacoes]
            };
        }

        [HttpPost("cadastro")]
        public ApiResponse<bool> EnviarNotificacaoAsync([FromBody] NotificacaoSiteUsuarioDTO dto)
        {
            if (dto.Usuario.Id == 0)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "ID do usuário destinatário inválido",
                    Data = false
                };
            }

            var usuarioExiste = _context.Usuarios.Find(dto.Usuario.Id);

            if (usuarioExiste == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Usuário destinatário não encontrado",
                    Data = false
                };
            }

            _context.NotificacoesSite.Add(dto.Notificacao);
            _context.SaveChanges();

            var nsu = new NotificacaoSiteUsuario
            {
                Usuario = usuarioExiste,
                Notificacao = dto.Notificacao
            };

            _context.NotificacoesSiteUsuarios.Add(nsu);
            _context.SaveChanges();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Notificação adicionada com sucesso",
                Data = true
            };
        }
    
        [HttpPost("cadastro/varias")]
        public ApiResponse<bool> EnviarNotificoesAsync([FromBody] NotificacaoSiteUsuariosDTO nsus)
        {
            // Cria apenas uma notificação
            _context.NotificacoesSite.Add(nsus.Notificacao);
            _context.SaveChanges();

            // Busca todos os usuários de uma vez
            var cpfs = nsus.Usuarios.Select(u => u.Cpf).Where(cpf => cpf != null).ToList();
            var usuariosDb = _context.Usuarios
                .Where(u => cpfs.Contains(u.Cpf))
                .ToList();

            if (usuariosDb.Count == 0)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Nenhum usuário encontrado",
                    Data = false
                };
            }

            // Cria as relações para todos os usuários encontrados
            foreach (var usuarioDb in usuariosDb)
            {
                var nsu = new NotificacaoSiteUsuario
                {
                    Usuario = usuarioDb,
                    Notificacao = nsus.Notificacao
                };
                _context.NotificacoesSiteUsuarios.Add(nsu);
            }

            _context.SaveChanges();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = $"Notificação enviada para {usuariosDb.Count} usuário(s)",
                Data = true
            };
        }

        [HttpPost("cadastro/global")]
        public ApiResponse<bool> EnviarNotificacaoGlobalAsync([FromBody] NotificacaoSite notificacao)
        {
            _context.NotificacoesSite.Add(notificacao);
            _context.SaveChanges();

            var usuarioList = _context.Usuarios.ToList();

            if (usuarioList.Count == 0)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Nenhum usuário cadastrado no sistema",
                    Data = false
                };
            }

            foreach (var usuario in usuarioList)
            {
                var nsu = new NotificacaoSiteUsuario
                {
                    Usuario = usuario,
                    Notificacao = notificacao
                };
                _context.NotificacoesSiteUsuarios.Add(nsu);
            }

            _context.SaveChanges();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = $"Notificação global enviada para {usuarioList.Count} usuário(s)",
                Data = true
            };
        }

        [HttpPost("marcar-como-lida")]
        public ApiResponse<bool> MarcarComoLida([FromBody] NotificacaoSiteUsuario notificacaoSiteUsuario)
        {
            var nsu = _context.NotificacoesSiteUsuarios
                    .FirstOrDefault(n => n.UsuarioId == notificacaoSiteUsuario.UsuarioId &&
                                    n.NotificacaoId == notificacaoSiteUsuario.NotificacaoId);

            if (nsu == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Notificação não encontrada",
                    Data = false
                };
            }

            nsu.Lida = true;
            _context.SaveChanges();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Notificação marcada como lida com sucesso",
                Data = true
            };
        }
    
        [HttpPost("apagar")]
        public ApiResponse<bool> ApagarNotificacao([FromBody] NotificacaoSiteUsuario notificacaoSiteUsuario)
        {
            var nsu = _context.NotificacoesSiteUsuarios
                    .FirstOrDefault(n => n.UsuarioId == notificacaoSiteUsuario.UsuarioId &&
                                    n.NotificacaoId == notificacaoSiteUsuario.NotificacaoId);

            if (nsu == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Notificação não encontrada",
                    Data = false
                };
            }

            var temOutrasNotificacoes = _context.NotificacoesSiteUsuarios
                .Any(n => n.NotificacaoId == nsu.NotificacaoId && n.UsuarioId != nsu.UsuarioId);

            if (!temOutrasNotificacoes)
            {
                _context.NotificacoesSite.Remove(nsu.Notificacao);
            }

            _context.NotificacoesSiteUsuarios.Remove(nsu);
            _context.SaveChanges();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Notificação apagada com sucesso",
                Data = true
            };
        }
    }
}
