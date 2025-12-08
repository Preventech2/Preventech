namespace Preventech.Core.Models;


///<summary>
/// Tabela para controle das últimas alterações de cada outra tabela
///</summary>
public enum IndiceAtualizacao {
    ///<summary>
    /// Índice da última atualização da tabela de localizações
    ///</summary
    Localizacao = 0,

    ///<summary>
    /// Índice da última atualização da tabela de equipamentos
    ///</summary
    Equipamento = 1,

    Preditiva = 2,

    Preventiva
};