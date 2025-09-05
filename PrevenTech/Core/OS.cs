using System;

namespace PrevenTech.Core;

public class OS
{
    /// <summary>
    /// Data de abertura de OS
    /// </summary>
    public DateTime Abertura { get; private set; }

    /// <summary>
    /// Identificador de ordem de
    /// </summary>
    public long Identificador { get; private set; }


    public StatusOS Status { get; private set; }


}