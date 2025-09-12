using System;

namespace Preventech.Core.Models;

public class OrdemServico
{
    public DateTime Abertura { get; set; }
    public long Id { get; set; }

    public StatusOS Status { get; set; }

    public string Descricao { get; set; } = "";

    public string? TecnicoResponsavel { get; set; }
}