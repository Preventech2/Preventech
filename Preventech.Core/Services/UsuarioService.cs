using System;
using System.Net.Http.Json;
using Preventech.Core.Models;
using Preventech.Core.DTOs;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Preventech.Core.Services;

public class UsuarioService(HttpClient httpClient, ILogger<UsuarioService> logger)
{
    private readonly ILogger<UsuarioService> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<Usuario>> AddUsuarioAsync(Usuario usuario)
    {
        var response = await _httpClient.PostAsJsonAsync("api/usuarios/cadastro", usuario);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Usuario>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Usuario>.");
        return result;
    }

    public async Task<ApiResponse<Usuario>> AddGrupoInUsuarioAsync(Usuario usuario)
    {
        var response = await _httpClient.PostAsJsonAsync("api/usuarios/add-grupo", usuario);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Usuario>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Usuario> while adding group.");
        return result;
    }

    public async Task<ApiResponse<Usuario>> LoginUsuarioAsync(Usuario usuario)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/usuarios/login", usuario);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Usuario>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Usuario> while login.");

        return result;
    }
    
    public async Task<ApiResponse<List<Usuario>>> GetUsuariosAsync()
    {
        var response = await _httpClient.GetAsync("api/usuarios");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Usuario>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Usuario>>.");
        return result;
    }

    public async Task<ApiResponse<Usuario>> GetUsuarioByCpfAsync(Usuario usuario)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/usuarios/", usuario);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Usuario>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Usuario>.");
        return result;
    }

    public async Task<ApiResponse<Usuario>> InviteUsuarioAsync(Usuario usuario)
    {
        var response = await _httpClient.PostAsJsonAsync("api/usuarios/invite", usuario);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Usuario>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<Usuario>.");
        return result;
    }
}
