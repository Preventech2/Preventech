using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(11)]
    public string? Cpf { get; set; }

    [MaxLength(50)]
    public string? Nome { get; set; }

    [EmailAddress]
    [MaxLength(50)]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Senha { get; set; }

    public GrupoPerfil? Grupo { get; set; }
}
