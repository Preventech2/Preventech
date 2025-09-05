using System;

namespace PrevenTech.Core;

public sealed class Localizacao(int campus, int predio, int andar, int numero)
{
    /// <summary>
    /// Número do campus
    /// </summary>
    public int Campus { get; set; } = campus;

    /// <summary>
    /// Número do prédio
    /// </summary>
    public int Predio { get; set; } = predio;

    /// <summary>
    /// Número do andar
    /// </summary>
    public int Andar { get; set; } = andar;

    /// <summary>
    /// Número da sala
    /// </summary>
    public int Numero { get; set; } = numero;

    public override string ToString() => $"c{Campus} p{Predio} sala {Numero} {Andar}º andar";
    
}
