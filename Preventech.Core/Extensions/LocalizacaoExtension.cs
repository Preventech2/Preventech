using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Preventech.Core.Models;

namespace Preventech.Core.Extensions;

public class LocalizacaoExtension
{
    private static void CompactarRaw(Localizacao loc, Localizacao filtro, DateTime atualizado, ref MemoryStream bs) {
        if (atualizado >= loc.AtualizadoEm) return;
        var tmp = new byte[4];

        BinaryPrimitives.WriteInt32LittleEndian(tmp, loc.Id);
        bs.Write(tmp);

        if (string.IsNullOrEmpty(filtro.Apelido))
        {
            BinaryPrimitives.WriteInt32LittleEndian(tmp, Encoding.Default.GetByteCount(loc.Apelido!));
            bs.Write(tmp);
            bs.Write(Encoding.Default.GetBytes(loc.Apelido!));
        }

        if (filtro.Campus <= 0)
        {
            BinaryPrimitives.WriteInt32LittleEndian(tmp, loc.Campus);
            bs.Write(tmp);
        }

        if (filtro.Predio <= 0)
        {
            BinaryPrimitives.WriteInt32LittleEndian(tmp, loc.Predio);
            bs.Write(tmp);
        }

        if (filtro.Andar <= 0)
        {
            BinaryPrimitives.WriteInt32LittleEndian(tmp, loc.Andar);
            bs.Write(tmp);
        }

        if (filtro.Numero <= 0)
        {
            BinaryPrimitives.WriteInt32LittleEndian(tmp, loc.Numero);
            bs.Write(tmp);
        }

        if (filtro.Responsavel.Id <= 0)
        {
            BinaryPrimitives.WriteInt32LittleEndian(tmp, loc.Responsavel.Id);
            bs.Write(tmp);
        }
    }

    /// <summary>
    /// Usa conhecimento dos filtros presentes para compactar uma localização
    /// </summary>
    /// Ao passar um filtro (por exemplo, campus e prédio) à um getter, 
    /// todos os elementos retornados terão esses membros repetidos, logo
    /// eles são removidos causando uma diminuição na quantidade de tráfego.
    public static string Compactar(Localizacao loc, Localizacao filtro, DateTime atualizado)
    {
        var bs = new MemoryStream();
        CompactarRaw(loc, filtro, atualizado, ref bs);
        var res = Convert.ToBase64String(bs.ToArray());
        bs.Close();
        return res;
    }

    public static string Compactar(ICollection<Localizacao> locs, Localizacao filtro, DateTime atualizado) {
        var res = new MemoryStream();
        using var cmp = new MemoryStream();
        var tmp = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(tmp, locs.Count);
        res.Write(tmp);

        foreach (var loc in locs)
            CompactarRaw(loc, filtro, atualizado, ref res);
        
        using var compressor = new DeflateStream(cmp, CompressionMode.Compress);
        res.CopyTo(compressor);
        var b64 = Convert.ToBase64String(cmp.ToArray());
        res.Close();
        return b64;
    }

    /// <summary>
    /// Descompacta uma localização em Base64 com o conhecimento do filtro
    public static Localizacao Descompactar(string b64, Localizacao filtro)
    {
        var bytes = Convert.FromBase64String(b64);
        using var bs = new MemoryStream(bytes);
        var tmp = new byte[4];

        var resultado = new Localizacao();

        bs.Read(tmp);
        resultado.Id = BinaryPrimitives.ReadInt32LittleEndian(tmp);

        if (string.IsNullOrEmpty(filtro.Apelido))
        {
            bs.Read(tmp);
            var len = BinaryPrimitives.ReadInt32LittleEndian(tmp);
            resultado.Apelido = Encoding.Default.GetString(bytes, 8, len);
        } else resultado.Apelido = filtro.Apelido;

        if (filtro.Campus <= 0)
        {
            bs.Read(tmp);
            resultado.Campus = BinaryPrimitives.ReadInt32LittleEndian(tmp);
        } else resultado.Campus = filtro.Campus;

        if (filtro.Predio <= 0)
        {
            bs.Read(tmp);
            resultado.Predio = BinaryPrimitives.ReadInt32LittleEndian(tmp);
        } else resultado.Predio = filtro.Predio;

        if (filtro.Andar <= 0)
        {
            bs.Read(tmp);
            resultado.Andar = BinaryPrimitives.ReadInt32LittleEndian(tmp);
        } else resultado.Andar = filtro.Andar;

        if (filtro.Numero <= 0) 
        {
            bs.Read(tmp);
            resultado.Numero = BinaryPrimitives.ReadInt32LittleEndian(tmp);
        } else resultado.Numero = filtro.Numero;    

        if (filtro.Responsavel.Id <= 0)
        {
            bs.Read(tmp);
            resultado.Responsavel.Id = BinaryPrimitives.ReadInt32LittleEndian(tmp);
        } else resultado.Responsavel.Id = filtro.Responsavel.Id;

        return resultado;
    }

    public static List<Localizacao> DescompactarVarios(string b64, Localizacao filtro) {
        var barr = Convert.FromBase64String(b64);
        var ms = new MemoryStream(barr);
        var tmp = new byte[4];
        ms.Read()
    }
}