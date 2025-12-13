using System;

namespace Preventech.Core.Interfaces;

public interface INotificacao
{
    string Titulo { get; set; }
    string Mensagem { get; set; }
}
