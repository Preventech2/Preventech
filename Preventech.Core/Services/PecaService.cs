using System;
using System.Net.Http.Json;
using Preventech.Core.Models;
using Preventech.Core.DTOs;

namespace Preventech.Core.Services;

public class PecaService
{
    private readonly HttpClient _httpClient;

    public PecaService(HttpClient httpCLient)
    {
        _httpClient = httpCLient;
    }

    public async Task<ApiResponse<List<Peca>>> GetPecasAsync()
    {
        var response = await _httpClient.GetAsync("api/peca");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Peca>>>() ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Peca>>.");
        return result;
    }

    public async Task<ApiResponse<Peca>> GetPecaByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/peca/{id}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Peca>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Peca>.");
        return result;
    }

    public async Task<ApiResponse<Peca>?> AddPecaAsync(Peca peca)
    {
        var response = await _httpClient.PostAsJsonAsync("api/peca/cadastro", peca);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Peca>>() ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Peca>");
        return result;
    }

    public async Task<ApiResponse<Peca>?> UpdatePeca(Guid id, Peca peca)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/peca/editar/{id}", peca);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Peca>>() ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Peca>");
        return result;
    }

    public async Task DeletePecaAsync(long id)
    {
        var response = await _httpClient.DeleteAsync($"api/peca/excluir/{id}");
        response.EnsureSuccessStatusCode();
    }
}