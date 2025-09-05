using System;

namespace PrevenTech.Core;

public class Maquina(Localizacao local, string nome, string patrimonio, ulong id)
{
    public Localizacao Local { get; set; } = local;
    public string Nome { get; set; } = nome;
    public string Patrimonio { get; set; } = patrimonio;
    public ulong Identificador { get; set; } = id;

    /// id-preventiva (1)
    /// id-preditiva (*)
    /// 
    /// 
    /// 
    public override string ToString() => $"{Nome}: patrimonio ({Patrimonio}) em {Local.ToString()}";
}
