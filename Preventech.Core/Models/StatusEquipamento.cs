namespace Preventech.Core.Models;

public enum StatusEquipamento
{
    /// <summary>
    /// Equipamento não funciona
    /// </summary>
    Inoperante,

    /// <summary>
    /// Equipamento funcionando com alguma avaria
    /// </summary>
    ComAvaria,

    /// <summary>
    /// Equipamento com todas as manutenções em dia
    /// </summary>
    ManutencaoEmDia,

    /// <summary>
    /// Equipamento funciona, as manutenções estão atrasadas
    /// </summary>
    Funcionando,

    /// <summary>
    /// Status para filtros e máquinas recém cadastradas
    /// </summary>
    Invalido
}