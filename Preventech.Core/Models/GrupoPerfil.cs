using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class GrupoPerfil
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(100)]
    public string? Nome { get; set; }

    public Perfil? Permissoes { get; set; } = Perfil.NenhumaPermissao;
}
