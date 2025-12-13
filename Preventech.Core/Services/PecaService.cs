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
        try
        {
            // 1. Use GetAsync em vez de GetFromJsonAsync para pegar a resposta crua
            var response = await _httpClient.GetAsync("api/peca"); // Verifique se a rota é essa mesma

            // 2. Verifique se a requisição deu certo (Status 200-299)
            if (!response.IsSuccessStatusCode)
            {
                // Se deu erro (ex: 404, 500), retorne um objeto vazio ou com mensagem de erro
                Console.WriteLine($"Erro na API de Peças: {response.StatusCode}");
                return new ApiResponse<List<Peca>> 
                { 
                    Success = false, 
                    Message = $"Erro ao buscar peças: {response.StatusCode}",
                    Data = new List<Peca>() 
                };
            }

            // 3. Verifique se o conteúdo está vazio (Status 204 No Content)
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new ApiResponse<List<Peca>> { Data = new List<Peca>(), Success = true };
            }

            // 4. Agora é seguro tentar converter para JSON
            return await response.Content.ReadFromJsonAsync<ApiResponse<List<Peca>>>() 
                ?? new ApiResponse<List<Peca>> { Data = new List<Peca>() };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exceção ao buscar peças: {ex.Message}");
            return new ApiResponse<List<Peca>> { Success = false, Message = ex.Message };
        }
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

    public async Task<ApiResponse<Peca>?> UpdatePecaAsync(Guid id, Peca peca)
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