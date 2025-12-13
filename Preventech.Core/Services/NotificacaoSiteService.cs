using System;
using System.Net.Http.Json;
using Preventech.Core.Models;
using Preventech.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;

namespace Preventech.Core.Services;

public class NotificacaoSiteService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResponse<List<NotificacaoSiteUsuario>>> GetNotificacoesAsync(Usuario usuario)
    {
        var response = await _httpClient.PostAsJsonAsync("api/notificacoes", usuario);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<NotificacaoSiteUsuario>>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<NotificacaoSiteUsuario>>.");
        return result;
    }

    public async Task<ApiResponse<bool>> EnviarNotificacaoAsync(NotificacaoSite notificacao, Usuario destinatario)
    {
        notificacao.DataPublicacao = DateTime.UtcNow;
        notificacao.DataExpiracao = DateTime.UtcNow.AddDays(30);

        NotificacaoSiteUsuarioDTO nsu = new(destinatario, notificacao);

        var response = await _httpClient.PostAsJsonAsync("api/notificacoes/cadastro", nsu);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            
            return new ApiResponse<bool>
            {
                Success = false,
                Message = $"Erro HTTP {response.StatusCode}: {errorContent}",
                Data = false
            };
        }
        
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<bool>.");
        return result;
    }

    public async Task<ApiResponse<bool>> EnviarNotificacoesAsync(NotificacaoSite notificacao, List<Usuario> destinatarios)
    {
        notificacao.DataPublicacao = DateTime.UtcNow;
        notificacao.DataExpiracao = DateTime.UtcNow.AddDays(30);

        NotificacaoSiteUsuariosDTO nsus = new(destinatarios, notificacao);

        var response = await _httpClient.PostAsJsonAsync("api/notificacoes/cadastro/varias", nsus);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<bool>.");
        return result;
    }

    public async Task<ApiResponse<bool>> EnviarNotificacaoGlobalAsync(NotificacaoSite notificacao)
    {
        notificacao.DataPublicacao = DateTime.UtcNow;
        notificacao.DataExpiracao = DateTime.UtcNow.AddDays(30);

        var response = await _httpClient.PostAsJsonAsync("api/notificacoes/cadastro/global", notificacao);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<bool>.");
        return result;
    }

    public async Task<ApiResponse<bool>> MarcarComoLidaAsync(NotificacaoSiteUsuario notificacao)
    {
        var response = await _httpClient.PostAsJsonAsync("api/notificacoes/marcar-como-lida", notificacao);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<bool>.");
        return result;
    }

    public async Task<ApiResponse<bool>> ApagarNotificacaoAsync(NotificacaoSiteUsuario notificacaoSiteUsuario)
    {
        var response = await _httpClient.PostAsJsonAsync("api/notificacoes/apagar", notificacaoSiteUsuario);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>()
        ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<bool>.");
        return result;
    }
}
