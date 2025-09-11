using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Equipamento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string? Nome { get; set; } = "";

    [Required]
    [MaxLength(120)]
    public string? Patrimonio { get; set; } = "";

    [Required]
    public string Local { get; set; } = "";

    /// id-preventiva (*)
    /// id-preditiva (*)
    /// 
    /// 
    /// 
    public override string ToString() => $"{Nome}<patrimonio ({Patrimonio}) em {Local}>";
}
