using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Preventech.Core.Interfaces;

namespace Preventech.Core.Models;

public class NotificacaoSite : INotificacao
{
    public NotificacaoSite() { }
    public NotificacaoSite(string titulo, string mensagem)
    {
        Titulo = titulo;
        Mensagem = mensagem;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [StringLength(80)]
    public string Titulo { get; set; } = "";
    [StringLength(500)]
    public string Mensagem { get; set; } = "";
    public DateTime DataPublicacao { get; set; }
    public DateTime DataExpiracao { get; set; }
}
