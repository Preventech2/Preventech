using System;

namespace Preventech.Core.Models;

public enum GrupoPerfil1
{
    Gestor = Perfil.All,
    Tecnico =   Perfil.CriarEquipamento | Perfil.EditarEquipamento | Perfil.VisualizarEquipamento |
                Perfil.VisualizarLaboratorio |
                Perfil.CriarOrdemDeServico | Perfil.EditarOrdemDeServico | Perfil.VisualizarOrdemDeServico,
    UsuarioComum =  Perfil.VisualizarEquipamento |
                    Perfil.VisualizarLaboratorio |
                    Perfil.CriarOrdemDeServico | Perfil.VisualizarOrdemDeServico,
    Convidado =     Perfil.CriarOrdemDeServico | Perfil.VisualizarOrdemDeServico,
}
