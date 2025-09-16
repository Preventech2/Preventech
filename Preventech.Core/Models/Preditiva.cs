using System;

namespace Preventech.Core.Models;

public class Preditiva
{
    /// <summary>
    /// Representa a frequência de manutenção preditiva
    /// </summary>
    public DateTimeOffset Frequencia { get; set; }

    /// <summary>
    /// Equipamento referente à esta manutenção
    /// </summary>
    public Equipamento Referente { get; set; }
}
