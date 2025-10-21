using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

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
    [StringLength(120)]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Chave patrimonial
    /// </summary>
    [StringLength(120)]
    public string Patrimonio { get; set; } = string.Empty;

    /// <summary>
    /// Como o equipamento se encontra atualmente
    /// </summary>
    public StatusEquipamento Status { get; set; } = StatusEquipamento.Invalido;

    /// <summary>
    /// Localização do patrimônio
    /// </summary>
    public Localizacao Local { get; set; } = Localizacao.Vazia;

    /// <summary>
    /// Manutenção preventiva associada ao equipamento
    /// </summary>
    public ICollection<Preventiva>? ManutPreventiva { get; set; }

    /// <summary>
    /// Manutenções preditivas associadas ao equipamento
    /// </summary>
    public ICollection<Preditiva>? ManutPreditiva { get; set; }

    public override string ToString() => $"{Nome}<patrimonio ({Patrimonio}) em {Local}>";

    /// <summary>
    /// Equipamento vazio utilizado para passar sobre filtros
    /// </summary>
    public static readonly Equipamento Vazio = new();
}
