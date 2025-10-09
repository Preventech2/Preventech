using System;
using System.ComponentModel.DataAnnotations;

namespace Preventech.Core.Models;

public class GrupoPerfil
{
    [Key]
    [Required]
    [StringLength(100)]
    public string? Nome { get; set; }

    [Required]
    public Perfil? Permissoes { get; set; } = Perfil.NenhumaPermissao;
}
