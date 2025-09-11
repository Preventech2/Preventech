using System;

namespace Preventech.Core.Models;

public class Localizacao
{
    /// <summary>
    /// Número do campus
    /// </summary>
    public int Campus { get; set; }

    /// <summary>
    /// Número do prédio
    /// </summary>
    public int Predio { get; set; }

    /// <summary>
    /// Número do andar
    /// </summary>
    public int Andar { get; set; }

    /// <summary>
    /// Número da sala
    /// </summary>
    public int Numero { get; set; }

    public override string ToString() => $"c{Campus} p{Predio} sala {Numero} {Andar}º andar";
}
