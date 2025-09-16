using System;
using System.Net.Http.Json;
using Preventech.Core.Models;
using Preventech.Core.DTOs;

namespace Preventech.Core.Services;

public class EquipamentoService
{
    private readonly HttpClient _httpClient;

    public EquipamentoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<List<Equipamento>>> GetEquipamentosAsync()
    {
        var response = await _httpClient.GetAsync("api/equipamentos");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Equipamento>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Equipamento>>.");
        return result;
    }

    public async Task<Equipamento?> GetEquipamentoByIdAsync(string patrimonio)
    {
        return await _httpClient.GetFromJsonAsync<Equipamento>($"api/equipamentos/{patrimonio}");
    }

    public async Task<ApiResponse<Equipamento>?> AddEquipamentoAsync(Equipamento Equipamento)
    {
        var response = await _httpClient.PostAsJsonAsync("api/equipamentos/cadastro", Equipamento);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ApiResponse<Equipamento>>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Erro ao cadastrar máquina: {response.StatusCode} - {errorContent}");
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
