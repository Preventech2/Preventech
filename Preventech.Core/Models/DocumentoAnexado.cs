using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Preventech.Core.Models;

public class DocumentoAnexado
{
    /// <summary>
    /// Identificador de documento
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Data de envio do documento
    /// </summary>
    [Required]
    public DateTime DataEnvio { get; set; }
    public string? NomeArquivo { get; set; } // The original file name
    public string? TipoConteudo { get; set; } // The MIME type (e.g., "application/pdf")
    public string? Url { get; set; }
    public Guid OrdemServicoId { get; set; }

    [JsonIgnore]
    public OrdemServico? OrdemServico { get; set; }

    public DocumentoAnexado(string NomeArquivo, String TipoConteudo, string Url)
    {
        this.NomeArquivo = NomeArquivo;
        this.TipoConteudo = TipoConteudo;
        this.Url = Url;
        this.DataEnvio = DateTime.UtcNow;
        this.Id = GerarIdDocumento();
    }

    public static Guid GerarIdDocumento()
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
