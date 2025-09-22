using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Preditiva
{
    /// <summary>
    /// Identificador interno da manutenção preditiva
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "bigint")]
    public DateTimeOffset Frequencia { get; set; }

    /// <summary>
    /// Última vez que essa manutenção foi realizada
    /// </summary>
    public DateTime UltimaRealizacao { get; set; }

    /// <summary>
    /// Equipamento referente à esta manutenção
    /// </summary>
    public Equipamento? Referente { get; set; }

    public StatusManutencao Status { get; set; }
}
