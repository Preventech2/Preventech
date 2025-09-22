namespace Preventech.Core.Models;

//[Flags]
public enum Perfil
{
    NenhumaPermissao = 0,
    EditarEquipamento = 1 << 0,
    VisualizarEquipamento = 1 << 1,
    VisualizarLaboratorio = 1 << 2,
    VisualizarOrdemServico = 1 << 3,
}