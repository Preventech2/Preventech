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
        return await response.Content.ReadFromJsonAsync<ApiResponse<Usuario>>();
    }

    public async Task<string?> LoginUsuarioAsync(Usuario usuario)
    {
        var response = await _httpClient.PostAsJsonAsync("api/usuarios/login", usuario);
        return await response.Content.ReadFromJsonAsync<string>();
    }
}
