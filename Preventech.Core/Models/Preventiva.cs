using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Preventiva
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }

    /// <summary>
    /// Representa a frequência de manutenção preventiva
    /// </summary>

    [Required]
    public required DateTimeOffset Frequencia { get; set; }

    /// <summary>
    /// Arquivo que representa as boas práticas da manutenção 
    /// </summary>
    [Required]
    [MaxLength(240)]
    public required string ArquivoReferente { get; set; }
}
