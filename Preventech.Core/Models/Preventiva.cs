using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Preventiva
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Ultima realização da máquina referente
    /// </summary>
    public DateTime UltimaRealizacao { get; set; }

    /// <summary>
    /// Representa a frequência de manutenção preventiva
    /// </summary>
    
    public DateTimeOffset Frequencia { get; set; }

    /// <summary>
    /// Arquivo que representa as boas práticas da manutenção 
    /// </summary>
    [MaxLength(240)]
    public string? ArquivoReferente { get; set; }

    /// <summary>
    /// Equipamento referente à esta manutenção
    /// </summary>
    public Equipamento? Referente { get; set; }

    public StatusManutencao Status { get; set; }
}
