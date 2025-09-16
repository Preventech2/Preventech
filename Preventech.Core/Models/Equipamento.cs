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
    [Required]
    [MaxLength(120)]
    public required string Nome { get; set; } = "";

    /// <summary>
    /// Chave patrimonial
    /// </summary>
    [Required]
    [MaxLength(120)]
    public required string Patrimonio { get; set; } = "";

    /// <summary>
    /// Localização do patrimônio
    /// </summary>
    [Required]
    public required Localizacao Local { get; set; }

    /// <summary>
    /// Manutenção preventiva associada à máquina
    /// </summary>
    public Preventiva? ManutPreventiva { get; set; }

    /// <summary>
    /// Manutenções preditivas associadas à máquina
    /// </summary>
    public ICollection<Preditiva>? ManutPreditiva { get; set; }

    public override string ToString() => $"{Nome}<patrimonio ({Patrimonio}) em {Local}>";
}
