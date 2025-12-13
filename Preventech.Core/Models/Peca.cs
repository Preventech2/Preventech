using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Peca 
{
    /// <summary>
    /// Identificador de peca
    ///  </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Descrição da peça
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    ///  Quantidade da peça no estoque
    /// </summary>
    public int? Quantidade { get; set; }
}