using System;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using Preventech.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Preventech.Core.Services;

namespace Preventech.Server.SecurityServices;

public class AuthenticationStateService(ProtectedSessionStorage sessionStorage) : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage = sessionStorage;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var result = await _sessionStorage.GetAsync<Usuario>("usuario");
            var usuario = result.Success ? result.Value : null;

            // Cria uma ClaimsIdentity baseada no usuário recuperado
            ClaimsIdentity identity;

            if (usuario != null && !string.IsNullOrEmpty(usuario.Nome))
            {
                List<Claim> claims =
                [
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty)
                ];

                // Adiciona as permissões do usuário como claims
                foreach (Perfil perfil in Enum.GetValues<Perfil>())
                {
                    if (perfil != Perfil.NenhumaPermissao && usuario.Perfil?.HasFlag(perfil) == true)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, perfil.ToString()));
                    }
                }

                identity = new ClaimsIdentity(claims, "preventech_auth");
            }
            else
            {
                // Usuário não autenticado/logado
                identity = new ClaimsIdentity();
            }

            // Cria o ClaimsPrincipal e retorna o AuthenticationState
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
        catch
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
    }

    public async Task MarkUserAsAuthenticated(Usuario usuario)
    {
        await _sessionStorage.SetAsync("usuario", usuario);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _sessionStorage.DeleteAsync("usuario");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<Usuario?> GetUsuarioAsync()
    {
        try
        {
            var result = await _sessionStorage.GetAsync<Usuario>("usuario");
            var usuario = result.Success ? result.Value : null;

            return usuario;
        }
        catch
        {
            return null;
        }
    }
}
