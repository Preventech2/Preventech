using System;
using Preventech.Core.Interfaces;

namespace Preventech.Core.Models;

public class NotificacaoEmail : INotificacao
{
    public NotificacaoEmail() { }
    public NotificacaoEmail(string titulo, string mensagem, string destinatario)
    {
        Titulo = titulo;
        Mensagem = mensagem;
        Destinatario = destinatario;
    }

    public string Titulo { get; set; } = "";
    public string Mensagem { get; set; } = "";
    public string Destinatario { get; set; } = "";
}
