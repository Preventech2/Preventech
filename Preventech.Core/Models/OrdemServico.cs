using System;

namespace Preventech.Core.Models;

public class OrdemServico
{
    /// <summary>
    /// Data de abertura de OS
    /// </summary>
    public DateTime Abertura { get; private set; }

    /// <summary>
    /// Identificador de ordem de
    /// </summary>
    public long Id { get; private set; }


    public StatusOS Status { get; private set; }

}
