using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class OrdemServico
{
    /// <summary>
    /// Identificador de ordem de serviço
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Data de abertura de OS
    /// </summary>
    [Required]
    public DateTime Abertura { get; set; }

    /// <summary>
    /// Estado atual da OS
    /// </summary>
    public StatusOS Status { get; set; }

    public string Titulo { get; set; } = "";

    public string Descricao { get; set; } = "";

    public string? TecnicoResponsavel { get; set; }

    public static Guid GerarIdOrdem()
    {
        var agora = DateTimeOffset.Now;
        return new Guid(
            (int)agora.ToUnixTimeMilliseconds(),
            (short)agora.Year,
            (short)agora.Month,
            (byte)agora.Day,
            0, 0, 0,
            (byte)agora.Hour,
            (byte)agora.Minute,
            0,
            (byte)agora.Second);
    }
}
