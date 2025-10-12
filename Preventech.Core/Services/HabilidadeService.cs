using System;
using System.Net.Http.Json;
using Preventech.Core.DTOs;
using Preventech.Core.Models;

namespace Preventech.Core.Services;

public class HabilidadeService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<Habilidade>>> GetAllHabilidadesAsync()
    {
        var response = await _httpClient.GetAsync("api/habilidades");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Habilidade>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Habilidade>>.");
        return result;
    }

    public async Task<ApiResponse<List<Habilidade>>> AddHabilidadeAsync(Habilidade habilidade)
    {
        var response = await _httpClient.PostAsJsonAsync("api/habilidades/cadastro", habilidade);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Habilidade>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Habilidade>>.");
        return result;
    }

    public async Task<ApiResponse<Habilidade>> EditHabilidadeAsync(Habilidade habilidade)
    {
        var response = await _httpClient.PostAsJsonAsync("api/habilidades/editar", habilidade);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Habilidade>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Habilidade>.");
        return result;
    }

    public async Task<ApiResponse<Habilidade>> DeleteHabilidadeAsync(Habilidade habilidade)
    {
        var response = await _httpClient.PostAsJsonAsync("api/habilidades/deletar", habilidade);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Habilidade>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Habilidade>.");
        return result;
    }
}
