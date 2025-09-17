using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Equipamento
{
    /// <summary>
    /// Identificação interna do patrimônio 
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Nome do patrimônio
    /// </summary>
    [MaxLength(120)]
    public string? Nome { get; set; } = "";

    /// <summary>
    /// Chave patrimonial
    /// </summary>
    [MaxLength(120)]
    public string? Patrimonio { get; set; } = "";

    /// <summary>
    /// Localização do patrimônio
    /// </summary>
    public Localizacao? Local { get; set; }

    /// <summary>
    /// Manutenção preventiva associada ao equipamento
    /// </summary>
    public ICollection<Preventiva>? ManutPreventiva { get; set; }

    /// <summary>
    /// Manutenções preditivas associadas ao equipamento
    /// </summary>
    public ICollection<Preditiva>? ManutPreditiva { get; set; }

    public override string ToString() => $"{Nome}<patrimonio ({Patrimonio}) em {Local}>";
}
