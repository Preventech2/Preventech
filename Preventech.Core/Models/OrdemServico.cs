using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Preventech.Core.Services;

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

    public string? Observacoes { get; set; }
    
    public int? TecnicoResponsavelId { get; set; }
    public Usuario? TecnicoResponsavel { get; set; }

    public int? EquipamentoId { get; set; }
    public Equipamento? Equipamento { get; set; }
    
    public int? RequisitanteId { get; set; }
    public Usuario? Requisitante { get; set; }

    public List<Peca>? Pecas { get; set; } = new();
    
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
