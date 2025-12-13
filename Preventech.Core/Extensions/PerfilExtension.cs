using System;
using Preventech.Core.Models;

namespace Preventech.Core.Extensions;

public partial class PerfilExtension
{
    public const string NenhumaPermissao = "NenhumaPermissao";
    // Permissoes equipamentos
    public const string CriarEquipamento = "CriarEquipamento";
    public const string EditarEquipamento = "EditarEquipamento";
    public const string VisualizarEquipamento = "VisualizarEquipamento";
    // Permissoes laboratorios
    public const string CriarLaboratorio = "CriarLaboratorio";
    public const string EditarLaboratorio = "EditarLaboratorio";
    public const string VisualizarLaboratorio = "VisualizarLaboratorio";
    // Permissoes ordem de servico
    public const string CriarOrdemDeServico = "CriarOrdemDeServico";
    public const string EditarOrdemDeServico = "EditarOrdemDeServico";
    public const string VisualizarOrdemDeServico = "VisualizarOrdemDeServico";
    public const string PossuirHabilidades = "PossuirHabilidades";
    // Permissao de admin
    public const string Admin = "Admin";
    public const string GerenciarEstoque = "GerenciarEstoque";

    public static string ToString(Perfil perfil)
    {
        if (perfil == Perfil.NenhumaPermissao)
            return NenhumaPermissao;

        var flags = Enum.GetValues<Perfil>()
            .Where(p => p != Perfil.NenhumaPermissao && perfil.HasFlag(p))
            .Select(p =>
            {
                return p switch
                {
                    Perfil.CriarEquipamento => CriarEquipamento,
                    Perfil.EditarEquipamento => EditarEquipamento,
                    Perfil.VisualizarEquipamento => VisualizarEquipamento,
                    Perfil.CriarLaboratorio => CriarLaboratorio,
                    Perfil.EditarLaboratorio => EditarLaboratorio,
                    Perfil.VisualizarLaboratorio => VisualizarLaboratorio,
                    Perfil.CriarOrdemDeServico => CriarOrdemDeServico,
                    Perfil.EditarOrdemDeServico => EditarOrdemDeServico,
                    Perfil.VisualizarOrdemDeServico => VisualizarOrdemDeServico,
                    Perfil.PossuirHabilidades => PossuirHabilidades,
                    Perfil.GerenciarEstoque => GerenciarEstoque,
                    Perfil.Admin => Admin,
                    _ => null
                };
            })
            .Where(s => s != null);

        return string.Join(", ", flags);
    }

    public static string ToBeautifulString(Perfil perfil)
    {
        if (perfil == Perfil.NenhumaPermissao)
            return NenhumaPermissao;

        var flags = Enum.GetValues<Perfil>()
            .Where(p => p != Perfil.NenhumaPermissao && perfil.HasFlag(p))
            .Select(p =>
            {
                return p switch
                {
                    Perfil.NenhumaPermissao => NenhumaPermissao,
                    Perfil.CriarEquipamento => "CadastrarEquipamentos",
                    Perfil.EditarEquipamento => "ModificarEquipamentos",
                    Perfil.VisualizarEquipamento => "VisualizarEquipamentos",
                    Perfil.CriarLaboratorio => "CadastrarLaboratórios",
                    Perfil.EditarLaboratorio => "PossuirLaboratórios",
                    Perfil.VisualizarLaboratorio => "VisualizarLaboratórios",
                    Perfil.CriarOrdemDeServico => "SolicitarOrdensDeServiço",
                    Perfil.EditarOrdemDeServico => "AceitarEResolverOrdensDeServiço",
                    Perfil.VisualizarOrdemDeServico => "AcompanharOrdensDeServiço",
                    Perfil.PossuirHabilidades => PossuirHabilidades,
                    Perfil.GerenciarEstoque => "Gerenciar Estoque",
                    Perfil.Admin => Admin,
                    _ => throw new ArgumentOutOfRangeException(nameof(perfil), perfil, null)
                };
            })
            .Where(s => s != null);

        string strPerfil = string.Join(", ", flags);

        // Adiciona espaço antes de letras maiúsculas que não estão no início da string
        // Por fim de otimização, utilizei o source generator do .NET 7 para gerar a 
        // regex em tempo de compilação
        return MyRegex().Replace(strPerfil, " $1");
    }

    [System.Text.RegularExpressions.GeneratedRegex("(\\B[A-Z])")]
    private static partial System.Text.RegularExpressions.Regex MyRegex();
}
