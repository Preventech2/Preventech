using System;
using System.Net.Http.Json;
using Preventech.Core.Models;
using Preventech.Core.DTOs;
using Quartz.Util;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;

namespace Preventech.Core.Services;

public class LocalizacaoService
{
    private readonly HttpClient _httpClient;

    public LocalizacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public static string Query(Localizacao loc) {
        var itens = new Dictionary<string, string>() { };
        if (!loc.Apelido.IsNullOrWhiteSpace())
            itens.Add("Apelido", loc.Apelido!);

        if (loc.Campus > 0)
            itens.Add("Campus", loc.Campus.ToString());

        if (loc.Predio > 0)
            itens.Add("Predio", loc.Predio.ToString());

        if (loc.Andar > 0)
            itens.Add("Andar", loc.Andar.ToString());

        if (loc.Numero > 0)
            itens.Add("Sala", loc.Numero.ToString());

        
        var res = string.Join("&", from item in itens select $"{item.Key}={item.Value}");
        if (res.IsNullOrWhiteSpace()) return res;

        return "?" + res;
    }

    public async Task<ApiResponse<List<Localizacao>>> GetLocalizacoes(Localizacao loc)
    {
        var query = Query(loc);
        var response = await _httpClient.GetAsync($"/api/localizacao{query}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Localizacao>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Equipamento>>.");
        return result;
    }

    public async Task<ApiResponse<Localizacao>?> AddLocalizacaoAsync(Localizacao local)
    {

        var response = await _httpClient.PostAsJsonAsync("api/localizacao", local);
        Console.WriteLine(response.StatusCode);
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

    public async Task<ApiResponse<Localizacao>> EditarLocalizacaoAsync(Localizacao loc) {
        var response = await _httpClient.PatchAsJsonAsync("api/localizacao/", loc);
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
