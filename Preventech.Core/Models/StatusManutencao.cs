namespace Preventech.Core.Models;

/// <summary>
/// Status do qual uma manutenção pode se encontrar
/// </summary>
public enum StatusManutencao
{
    /// <summary>
    /// Manutenção feita em dia
    /// </summary>
    EmDia,

    /// <summary>
    /// Manutenção requisitada para hoje
    /// </summary>
    ParaHoje,

    /// <summary>
    /// Manutenção atrasada
    /// </summary>
    Atrasada,
}
