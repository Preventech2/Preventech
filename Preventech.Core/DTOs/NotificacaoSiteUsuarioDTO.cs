using System;
using Preventech.Core.Models;

namespace Preventech.Core.DTOs;

public class NotificacaoSiteUsuarioDTO
{
    public NotificacaoSiteUsuarioDTO()
    {
        Usuario = new Usuario();
        Notificacao = new NotificacaoSite();
    }

    public NotificacaoSiteUsuarioDTO(Usuario usuario, NotificacaoSite notificacao)
    {
        Usuario = usuario;
        Notificacao = notificacao;
    }

    public Usuario Usuario { get; set; }
    public NotificacaoSite Notificacao { get; set; }
    public bool Lida { get; set; } = false;
}
