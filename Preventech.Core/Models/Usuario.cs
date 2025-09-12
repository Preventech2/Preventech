using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Usuario
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Nome { get; set; } = null!;

    [MaxLength(50)]
    public string Email { get; set; } = null!;

    [MaxLength(50)]
    public string Senha { get; set; } = null!;
}
