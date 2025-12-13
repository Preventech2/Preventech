namespace Preventech.Core.Models;

[Flags]
public enum Perfil
{
    NenhumaPermissao = 0,
    // Permissoes equipamentos
    CriarEquipamento = 1 << 0,
    EditarEquipamento = 1 << 1,
    VisualizarEquipamento = 1 << 2,
    // Permissoes laboratorios
    CriarLaboratorio = 1 << 3,
    EditarLaboratorio = 1 << 4,
    VisualizarLaboratorio = 1 << 5,
    // Permissoes ordem de servico
    CriarOrdemDeServico = 1 << 6,
    EditarOrdemDeServico = 1 << 7,
    VisualizarOrdemDeServico = 1 << 8,
    // Permissoes habilidades do usuario
    PossuirHabilidades = 1 << 9,
    GerenciarEstoque = 1 << 10,        // 16 bit 1 (1024)

    Admin = All,
    All = 0b1111111111111111, // 16bits
}