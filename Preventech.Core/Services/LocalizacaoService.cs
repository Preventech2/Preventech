using System;
using System.Net.Http.Json;
using Preventech.Core.Models;
using Preventech.Core.DTOs;

namespace Preventech.Core.Services;

public class LocalizacaoService
{
    private readonly HttpClient _httpClient;

    public LocalizacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<List<Localizacao>>> GetLocalizacoes()
    {
        var response = await _httpClient.GetAsync("api/localizacao");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Localizacao>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Equipamento>>.");
        return result;
    }

    public async Task<ApiResponse<List<Localizacao>>> GetLocalizacoesPorCampus(int campus)
    {
        var response = await _httpClient.GetAsync($"api/localizacao/{campus}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Localizacao>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Localizacao>>.");
        return result;
    }

    public async Task<ApiResponse<List<Localizacao>>> GetLocalizacoesPorPredio(int campus, int predio)
    {
        var response = await _httpClient.GetAsync($"api/localizacao/{campus}/{predio}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Localizacao>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Localizacao>>.");
        return result;
    }

    public async Task<ApiResponse<List<Localizacao>>> GetLocalizacoesPorAndar(int campus, int predio, int andar)
    {
        var response = await _httpClient.GetAsync($"api/localizacao/{campus}/{predio}/{andar}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Localizacao>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Localizacao>>.");
        return result;
    }

    public async Task<ApiResponse<Localizacao>?> AddLocalizacaoAsync(Localizacao local)
    {

        var response = await _httpClient.PostAsJsonAsync("api/localizacao/cadastro", local);
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

    public async Task<ApiResponse<int>?> AddLocalizacoesAsync(Localizacao final)
    {

        var response = await _httpClient.PostAsJsonAsync("api/localizacao/cadastro/range", final);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ApiResponse<int>>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Erro ao cadastrar equipamento: {response.StatusCode} - {errorContent}");
        }
    }

    public async Task UpdateEquipamentoAsync(Equipamento Equipamento)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/equipamentos/{Equipamento.Patrimonio}", Equipamento);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteEquipamentoAsync(string patrimonio)
    {
        var response = await _httpClient.DeleteAsync($"api/equipamentos/{patrimonio}");
        response.EnsureSuccessStatusCode();
    }
}
