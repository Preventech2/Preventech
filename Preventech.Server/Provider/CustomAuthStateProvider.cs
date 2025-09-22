using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Preventech.Core.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Preventech.Core.Services;
using Preventech.Core.Models;
using Preventech.Core.DTOs;

namespace Preventech.Server.Provider;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILogger<CustomAuthStateProvider> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UsuarioService _usuarioService;


    public CustomAuthStateProvider(IHttpContextAccessor httpContextAccessor, ILogger<CustomAuthStateProvider> logger, UsuarioService usuarioService)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _usuarioService = usuarioService;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            if (_httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(AuthConstants.CookieName))
            {
                var token = _httpContextAccessor.HttpContext.Request.Cookies[AuthConstants.CookieName];
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var claims = new List<Claim>();

                foreach (var claim in jwtToken.Claims)
                {
                    claims.Add(new Claim(claim.Type, claim.Value));
                }

                var principal = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(principal);

                _logger.LogInformation("estado de autentificação do usuário {User} concluida.", user.Identity?.Name);
                return Task.FromResult(new AuthenticationState(user));
            }
            _logger.LogInformation("estado de autentificação do usuário deslogado concluida.");
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }
        catch (Exception ex)
        {
            _logger.LogError("Error processing authentication state: {Message}", ex.Message);
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }
    }

    public async Task<string?> GetUsuarioTokenAsync(Usuario usuario)
    {
        ApiResponse<string> response = await _usuarioService.LoginUsuarioAsync(usuario);
        return response.Success ? response.Data : string.Empty;
    }

    public void NotifyUserAuthentication()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<Usuario> GetUsuarioAsync() {
        return new Usuario();
    }
}
