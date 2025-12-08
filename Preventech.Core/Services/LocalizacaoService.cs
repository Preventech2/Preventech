using System;
using System.Net.Http.Json;
using Preventech.Core.Models;
using Preventech.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization.Metadata;

namespace Preventech.Core.Services;

public class LocalizacaoService(HttpClient httpClient)
{
    public static string Query(Localizacao loc, DateTime tempo, string Prefixo = "")
    {
        var itens = new Dictionary<string, string>() { };
        if (!string.IsNullOrWhiteSpace(loc.Apelido))
            itens.Add($"{Prefixo}Apelido", loc.Apelido!);

        if (loc.Campus > 0)
            itens.Add($"{Prefixo}Campus", loc.Campus.ToString());

        if (loc.Predio > 0)
            itens.Add($"{Prefixo}Predio", loc.Predio.ToString());

        if (loc.Andar > 0)
            itens.Add($"{Prefixo}Andar", loc.Andar.ToString());

        if (loc.Numero > 0)
            itens.Add($"{Prefixo}Sala", loc.Numero.ToString());

        itens.Add($"atualizacao", tempo.ToString());

        var res = string.Join("&", from item in itens select $"{item.Key}={item.Value}");
        if (string.IsNullOrWhiteSpace(res)) return string.Empty;

        return "?" + res;
    }

    public async Task<DateTime> GetUltimaAtualizacaoAsync()
    {
        var response = await httpClient
            .GetAsync("/api/localizacao/timestamp")
            .ContinueWith(x => x.Result.Content.ReadFromJsonAsync<DateTime>())
            .Unwrap();
        return response;
    }

    public async Task<ApiResponse<List<string>>> GetLocalizacoes(Localizacao loc, DateTime tempo)
    {
        var query = Query(loc, tempo);
        var response = await httpClient.GetAsync($"/api/localizacao{query}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<string>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Equipamento>>.");
        return result;
    }

    public async Task<ApiResponse<Localizacao>?> AddLocalizacaoAsync(Localizacao local)
    {

        var response = await httpClient.PostAsJsonAsync("api/localizacao", local);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ApiResponse<Localizacao>>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Erro ao cadastrar localizacao: {response.StatusCode} - {errorContent}");
        }
    }

    public async Task<ApiResponse<Localizacao>?> EditarLocalizacaoAsync(Localizacao loc)
    {
        var response = await httpClient.PatchAsJsonAsync("api/localizacao/", loc);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ApiResponse<Localizacao>>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Erro ao cadastrar equipamento: {response.StatusCode} - {errorContent}");
        }
    }
}
