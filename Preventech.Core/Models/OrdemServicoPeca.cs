using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Preventech.Core.Models;

public class OrdemServicoPeca
{
    // Chaves Estrangeiras
    public Guid IdOrdemServico { get; set; }
    public Guid IdPeca { get; set; }
    
    public int QtdPecas { get; set; }



    [ForeignKey("IdOrdemServico")]
    [JsonIgnore]
    public virtual OrdemServico? OrdemServico { get; set; }

    [ForeignKey("IdPeca")]
    public virtual Peca? Peca { get; set; }
}