using System;
using System.Net.Http.Json;
using Preventech.Core.DTOs;
using Preventech.Core.Models;

namespace Preventech.Core.Services;

public class GrupoPerfilService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<GrupoPerfil>>> GetGruposPerfisAsync()
    {
        var response = await _httpClient.GetAsync($"api/grupos-perfis");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<GrupoPerfil>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<GrupoPerfil>>.");
        return result;
    }

    public async Task<ApiResponse<GrupoPerfil>> GetGrupoPerfilByNameAsync(string grupo)
    {
        var response = await _httpClient.GetAsync($"api/grupos-perfis/{grupo}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<GrupoPerfil>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<GrupoPerfil>.");
        return result;
    }

    public async Task<ApiResponse<GrupoPerfil>> GetGrupoPerfilByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/grupos-perfis/{id}");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<GrupoPerfil>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<GrupoPerfil>.");
        return result;
    }

    public async Task<ApiResponse<GrupoPerfil>?> EditGrupoPerfilAsync(GrupoPerfil grupoPerfil)
    {
        var response = await _httpClient.PostAsJsonAsync("api/grupos-perfis/editar", grupoPerfil);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<GrupoPerfil>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<GrupoPerfil>.");
        return result;
    }

    public async Task<ApiResponse<GrupoPerfil>?> AddGrupoPerfilAsync(GrupoPerfil grupoPerfil)
    {
        var response = await _httpClient.PostAsJsonAsync("api/grupos-perfis/cadastro", grupoPerfil);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<GrupoPerfil>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<GrupoPerfil>.");
        return result;
    }

    public async Task<ApiResponse<GrupoPerfil>?> DeleteGrupoPerfilAsync(GrupoPerfil grupoPerfil)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/grupos-perfis/deletar", grupoPerfil);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<GrupoPerfil>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<GrupoPerfil>.");
        return result;
    }
}
