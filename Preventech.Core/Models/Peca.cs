using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Preventech.Core.Models;

public class Peca 
{
    /// <summary>
    /// Identificador de peca
    ///  </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Descrição da peça
    /// </summary>
    [Required(ErrorMessage = "A descrição da peça é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    ///  Quantidade da peça no estoque
    /// </summary>
    public int? Quantidade { get; set; }

    /// <summary>
    /// Ordens de Serviço que usam esta peça.
    /// </summary>
    public List<OrdemServico> OrdensDeServico { get; set; } = new();
}