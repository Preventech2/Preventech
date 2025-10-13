using System;
using System.Net.Http.Json;
using Preventech.Core.DTOs;
using Preventech.Core.Models;

namespace Preventech.Core.Services;

public class HabilidadeSistemaService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<HabilidadeSistema>>> GetAllHabilidadesAsync()
    {
        var response = await _httpClient.GetAsync("api/habilidades-sistema");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<HabilidadeSistema>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<HabilidadeSistema>>.");
        return result;
    }

    public async Task<ApiResponse<HabilidadeSistema>> GetHabilidadeByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/habilidades-sistema/{id}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<HabilidadeSistema>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<HabilidadeSistema>.");
        return result;
    }

    public async Task<ApiResponse<HabilidadeSistema>> AddHabilidadeSistemaAsync(HabilidadeSistema habilidade)
    {
        var response = await _httpClient.PostAsJsonAsync("api/habilidades-sistema/cadastro", habilidade);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<HabilidadeSistema>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<HabilidadeSistema>.");
        return result;
    }

    public async Task<ApiResponse<HabilidadeSistema>> EditHabilidadeSistemaAsync(HabilidadeSistema habilidade)
    {
        var response = await _httpClient.PostAsJsonAsync("api/habilidades-sistema/editar", habilidade);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<HabilidadeSistema>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<HabilidadeSistema>.");
        return result;
    }

    public async Task<ApiResponse<HabilidadeSistema>> DeleteHabilidadeSistemaAsync(HabilidadeSistema habilidade)
    {
        var response = await _httpClient.PostAsJsonAsync("api/habilidades-sistema/deletar", habilidade);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<HabilidadeSistema>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<HabilidadeSistema>.");
        return result;
    }
}
