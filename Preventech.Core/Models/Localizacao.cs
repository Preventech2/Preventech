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
    /// Número do campus
    /// </summary>
    /// 
    public int Campus { get; set; }

    /// <summary>
    /// Número do prédio
    /// </summary>
    public int Predio { get; set; }

    /// <summary>
    /// Número do andar
    /// </summary>
    public int Andar { get; set; }

    /// <summary>
    /// Número da sala
    /// </summary>
    public int Numero { get; set; }

    public Localizacao() { }

    public Localizacao(int Campus, int Predio, int Andar, int Numero) {
        this.Campus = Campus;
        this.Predio = Predio;
        this.Andar = Andar;
        this.Numero = Numero;
    } 
    
    public override string ToString() => $"c{Campus}p{Predio}s{Andar}{(Numero < 10 ? "0" : "")}{Numero}";
}
