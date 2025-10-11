using System;
using System.ComponentModel.DataAnnotations;

namespace Preventech.Core.Models;

public class Habilidade
{
    [Key]
    public string? Nome { get; set; }

    public string? Descricao { get; set; }
}
