namespace Preventech.Core.Models;

/// <summary>
/// Status do qual uma ordem de serviço pode se encontrar
/// </summary>
public enum StatusOS
{
    /// <summary>
    /// Ordem de serviço ainda não foi concluída
    /// </summary>
    EmProgresso,

    /// <summary>
    /// Ordem de serviço completa
    /// </summary>
    Concluida,

    /// <summary>
    /// Ordem não foi aceita
    /// </summary>
    Deferida,


    EmImpedimento
}
