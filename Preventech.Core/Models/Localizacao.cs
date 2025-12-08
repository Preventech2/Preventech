using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Preventech.Core.Models;

/// <summary>
/// Localização de uma equipamento no campus
/// </summary>
[BindProperties]
public class Localizacao
{
    /// <summary>
    /// Identificador interno do equipamento
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Nome do qual essa localização em específico 
    /// </summary>
    [StringLength(80)]
    public string? Apelido { get; set; }

    /// <summary>
    /// Número do campus
    /// </summary>
    /// 
    public int Campus { get; set; } = 0;

    /// <summary>
    /// Número do prédio
    /// </summary>
    public int Predio { get; set; } = 0;

    /// <summary>
    /// Número do andar
    /// </summary>
    public int Andar { get; set; } = 0;

    /// <summary>
    /// Número da sala
    /// </summary>
    public int Numero { get; set; }

    /// <summary>
    /// Técnico responsável pela sala
    /// </summary>
    public Usuario Responsavel { get; set; } = new();

    /// <summary>
    /// Última atualização desta localização
    /// </summary>
    public DateTime AtualizadoEm { get; set; } = DateTime.Now;

    public Localizacao() { }

    public Localizacao(string? Apelido, int Campus, int Predio, int Andar, int Numero, Usuario Responsavel)
    {
        this.Apelido = Apelido;
        this.Campus = Campus;
        this.Predio = Predio;
        this.Andar = Andar;
        this.Numero = Numero;
        this.Responsavel = Responsavel;
    }

    public override string ToString() => $"{Apelido ?? "s/n"} gerido por {Responsavel.Nome}";

    /// <summary>
    /// Localização vazia utilizada para passar sobre filtros
    /// </summary>
    public static readonly Localizacao Vazia = new();
}
