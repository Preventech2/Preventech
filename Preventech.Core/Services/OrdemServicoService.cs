using System;
using System.Net.Http.Json;
using Preventech.Core.Models;
using Preventech.Core.DTOs;

namespace Preventech.Core.Services;

public class OrdemServicoService
{
    private readonly HttpClient _httpClient;

    public OrdemServicoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Example method to get a list of OrdemServico
    public async Task<ApiResponse<List<OrdemServico>>> GetOrdemServicosAsync()
    {
        var response = await _httpClient.GetAsync("api/ordem-servico");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<OrdemServico>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<OrdemServico>>.");
        return result;
    }

    // Example method to get a single OrdemServico by ID

    public async Task<ApiResponse<OrdemServico>> GetOrdemServicoByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/ordem-servico/{id}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrdemServico>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<OrdemServico>.");
        return result;
    }

    public async Task<ApiResponse<List<OrdemServico>>> GetOrdemServicoByResponsavelAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/ordem-servico/user/{id}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<OrdemServico>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<OrdemServico>.");
        return result;
    }

    // Example method to create a new OrdemServico
    public async Task<ApiResponse<OrdemServico>?> AddOrdemServicoAsync(OrdemServico OS)
    {
        var response = await _httpClient.PostAsJsonAsync("api/ordem-servico/cadastro", OS);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrdemServico>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<OrdemServico>.");
        return result;
    }

    // Example method to update an existing OrdemServico
    public async Task<ApiResponse<OrdemServico>?> UpdateOrdemServicoAsync(Guid id, OrdemServico ordemServico)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/ordem-servico/{id}", ordemServico);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrdemServico>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<OrdemServico>.");

        return result;
    }

    // Example method to delete an OrdemServico
    public async Task DeleteOrdemServicoAsync(long id)
    {
        var response = await _httpClient.DeleteAsync($"api/ordem-servico/{id}");
        response.EnsureSuccessStatusCode();
    }
}