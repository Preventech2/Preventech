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

    public async Task<ApiResponse<List<Habilidade>>> GetAllHabilidadesByUsuarioAsync(Usuario usuario)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/habilidades/", usuario);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Habilidade>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Habilidade>>.");
        return result;
    }

    public async Task<ApiResponse<Habilidade>> GetHabilidadeByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/habilidades/{id}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Habilidade>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Habilidade>.");
        return result;
    }

    public async Task<ApiResponse<Habilidade>> AddHabilidadeAsync(Habilidade habilidade)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/habilidades/cadastro", habilidade);
            Console.WriteLine($"Response: {response}");

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error Content: {errorContent}");

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<Habilidade>>()
            ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Habilidade>.");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao adicionar habilidade: " + ex.Message);
            return new ApiResponse<Habilidade>
            {
                Success = false,
                Message = $"Erro ao adicionar habilidade: {ex.Message}",
                Data = null
            };
        }
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
