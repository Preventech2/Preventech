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
    public async Task<IEnumerable<OrdemServico>> GetOrdemServicosAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<OrdemServico>>("api/ordemservicos") ?? Enumerable.Empty<OrdemServico>();
    }

    // Example method to get a single OrdemServico by ID
    public async Task<OrdemServico?> GetOrdemServicoByIdAsync(long id)
    {
        return await _httpClient.GetFromJsonAsync<OrdemServico>($"api/ordemservicos/{id}");
    }

    // Example method to create a new OrdemServico
    public async Task<ApiResponse<OrdemServico>?> AddOrdemServicoAsync(OrdemServico ordemServico)
    {
        var response = await _httpClient.PostAsJsonAsync("api/ordemservicos", ordemServico);
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ApiResponse<OrdemServico>>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Erro ao cadastrar Ordem de Serviço: {response.StatusCode} - {errorContent}");
        }
    }

    // Example method to update an existing OrdemServico
    public async Task UpdateOrdemServicoAsync(long id, OrdemServico ordemServico)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/ordemservicos/{id}", ordemServico);
        response.EnsureSuccessStatusCode();
    }

    // Example method to delete an OrdemServico
    public async Task DeleteOrdemServicoAsync(long id)
    {
        var response = await _httpClient.DeleteAsync($"api/ordemservicos/{id}");
        response.EnsureSuccessStatusCode();
    }
}