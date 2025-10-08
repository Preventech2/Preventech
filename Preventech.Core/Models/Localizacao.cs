using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

/// <summary>
/// Localização de uma equipamento no campus
/// </summary>
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
    public string? Apelido { get; set; } = string.Empty;

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
    public int Numero { get; set; } = 0;

    /// <summary>
    /// Técnico responsável pela sala
    /// </summary>
    public Usuario? Responsável { get; set; } = default;

    public Localizacao() { }

    public Localizacao(string Apelido, int Campus, int Predio, int Andar, int Numero) {
        this.Apelido = Apelido;
        this.Campus = Campus;
        this.Predio = Predio;
        this.Andar = Andar;
        this.Numero = Numero;
    } 
    
    public override string ToString() => $"{(Apelido != null ? $"\"{Apelido}\" em " : "")}c{Campus}p{Predio}s{(Numero < 10 ? "0" : "")}{Numero}";
}
