using System;
using System.Net.Http.Json;
using Preventech.Core.Models;
using Preventech.Core.DTOs;

namespace Preventech.Core.Services;

public class UsuarioService
{
    private readonly HttpClient _httpClient;

    public UsuarioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<Usuario>> AddUsuarioAsync(Usuario usuario)
    {
        var response = await _httpClient.PostAsJsonAsync("api/usuarios/cadastro", usuario);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Usuario>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Usuario>.");
        return result;
    }

    public async Task<ApiResponse<Usuario>> LoginUsuarioAsync(Usuario usuario)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/usuarios/login", usuario);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Usuario>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Usuario>.");
        return result;
    }

    public async Task<ApiResponse<List<Usuario>>> GetUsuariosAsync()
    {
        var response = await _httpClient.GetAsync("api/usuarios");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Usuario>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Usuario>>.");
        return result;
    }
}
