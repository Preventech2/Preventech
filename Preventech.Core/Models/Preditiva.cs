using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Preditiva
{
    /// <summary>
    /// Representa a frequência de manutenção preditiva
    /// </summary>
    /// 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTimeOffset Frequencia { get; set; }

    /// <summary>
    /// Equipamento referente à esta manutenção
    /// </summary>
    public Equipamento? Referente { get; set; }
}
