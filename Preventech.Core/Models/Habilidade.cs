using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Habilidade
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public HabilidadeSistema? Categoria { get; set; } = new HabilidadeSistema();

    [Required]
    public Usuario? Usuario { get; set; } = new Usuario();

    [Required]
    [MaxLength(200)]
    public string? Descricao { get; set; } = string.Empty;
}
