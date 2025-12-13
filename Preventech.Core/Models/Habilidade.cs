using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Habilidade
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public HabilidadeSistema? Categoria { get; set; } = null;

    public Usuario? Usuario { get; set; } = null;

    [MaxLength(200)]
    public string? Descricao { get; set; } = string.Empty;
}
