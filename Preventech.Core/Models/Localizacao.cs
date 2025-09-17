using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

/// <summary>
/// Localização de uma máquina no campus
/// </summary>
public class Localizacao
{
    /// <summary>
    /// Número do campus
    /// </summary>
    /// 

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

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

    public override string ToString() => $"c{Campus} p{Predio} sala {Numero} {Andar}º andar";
}
