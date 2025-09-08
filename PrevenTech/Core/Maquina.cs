using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace PrevenTech.Core;

public class Maquina
{
    [Required]
    public Localizacao Local { get; set; } = new Localizacao();

    [Required]
    [MaxLength(120)]
    public string? Nome { get; set; } = "";

    [Required]
    [MaxLength(96)]
    public string? Patrimonio { get; set; } 

    [Required]
    public ulong Identificador { get; set; }

    

    /// id-preventiva (1)
    /// id-preditiva (*)
    /// 
    /// 
    /// 
    public override string ToString() => $"{Nome}<patrimonio ({Patrimonio}) em {Local}>";
}
