using System;
using Preventech.Core.Models;

namespace Preventech.Core.DTOs;

public class NotificacaoSiteUsuariosDTO
{
    public NotificacaoSiteUsuariosDTO()
    {
        Usuarios = [];
        Notificacao = new NotificacaoSite();
    }

    public NotificacaoSiteUsuariosDTO(List<Usuario> usuarios, NotificacaoSite notificacao)
    {
        Usuarios = usuarios;
        Notificacao = notificacao;
    }

    public List<Usuario> Usuarios { get; set; }
    public NotificacaoSite Notificacao { get; set; }
    public bool Lida { get; set; } = false;
}
