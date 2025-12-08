namespace Preventech.Core.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

[BindProperties]
public class Atualizacao {
    /// <summary>
    /// Identificador da atualização
    /// </summary>
    [Key]
    public IndiceAtualizacao Id { get; set; }

    /// <summary>
    /// Última atualização na tabela referente ao id
    /// </summary>
    public DateTime Ultima { get; set; }
}