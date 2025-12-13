using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Preventech.Core.Models;

[PrimaryKey(nameof(UsuarioId), nameof(NotificacaoId))]
public class NotificacaoSiteUsuario
{
    public NotificacaoSiteUsuario() 
    { 
        Usuario = new Usuario();
        Notificacao = new NotificacaoSite();
    }

    public NotificacaoSiteUsuario(int usuarioId, int notificacaoId)
    {
        UsuarioId = usuarioId;
        NotificacaoId = notificacaoId;
        Usuario = new Usuario();
        Notificacao = new NotificacaoSite();
    }

    public NotificacaoSiteUsuario(Usuario usuario, NotificacaoSite notificacao)
    {
        UsuarioId = usuario.Id;
        NotificacaoId = notificacao.Id;
        Usuario = usuario;
        Notificacao = notificacao;
    }

    public int UsuarioId { get; set; }
    public int NotificacaoId { get; set; }
    public Usuario Usuario { get; set; }
    public NotificacaoSite Notificacao { get; set; }
    public bool Lida { get; set; } = false;
}
