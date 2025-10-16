using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Preventech.Core.Models;

namespace Preventech.Core.Services;

public class UserClaimsHelper(AuthenticationStateProvider authStateProvider)
{
    private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;

    /// <summary>
    /// Gets the current user's information from claims
    /// </summary>
    /// <returns>UserInfo object with Name and Cpf, or null if not authenticated</returns>
    public async Task<UserInfo> GetCurrentUserInfoAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            return new UserInfo { IsAuthenticated = false };
        }
       
        return ExtractUserInfoFromClaims(user);
    }

    /// <summary>
    /// Extracts user information from ClaimsPrincipal
    /// </summary>
    /// <param name="user">The ClaimsPrincipal containing user claims</param>
    /// <returns>UserInfo object with Name, Cpf, and Id</returns>
    public static UserInfo ExtractUserInfoFromClaims(ClaimsPrincipal user)
    {
        var name = user.Identity?.Name 
            ?? user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
            ?? string.Empty;

        var cpf = user.Claims.FirstOrDefault(c => c.Type == "Cpf")?.Value
            ?? string.Empty;

        var email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
            ?? string.Empty;

        var nomeGrupo = user.Claims.FirstOrDefault(c => c.Type == "Grupo")?.Value
            ?? string.Empty;

        var idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var userId = !string.IsNullOrEmpty(idClaim) && int.TryParse(idClaim, out var id) ? id : 0;

        var roles = user.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        GrupoPerfil Grupo = new()
        {
            Nome = nomeGrupo,
            Permissoes = Perfil.NenhumaPermissao
        };

        foreach(var role in roles)
        {
            if (Enum.TryParse<Perfil>(role, out var perfil))
            {
                Grupo.Permissoes |= perfil;
            }
        }

        return new UserInfo
        {
            Id = userId,
            Cpf = cpf,
            Nome = name,
            Email = email,
            Grupo = Grupo,
            Roles = roles,
            IsAuthenticated = user.Identity?.IsAuthenticated ?? false
        };
    }

    /// <summary>
    /// Gets user information synchronously from ClaimsPrincipal (useful in controllers)
    /// </summary>
    /// <param name="user">The ClaimsPrincipal from HttpContext.User</param>
    /// <returns>UserInfo object with Name and Cpf</returns>
    public static UserInfo GetUserInfo(ClaimsPrincipal user)
    {
        return ExtractUserInfoFromClaims(user);
    }
}

/// <summary>
/// Contains user information extracted from claims
/// </summary>
public class UserInfo
{
    public int Id { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public GrupoPerfil Grupo { get; set; } = new GrupoPerfil();
    public List<string> Roles { get; set; } = [];
    public bool IsAuthenticated { get; set; }

    /// <summary>
    /// Checks if user has a specific role
    /// </summary>
    /// <param name="role">Role name to check</param>
    /// <returns>True if user has the role</returns>
    public bool HasRole(string role)
    {
        // As Roles são strings do Perfil, ex.: "VisualizarEquipamento"
        return Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets a display name for the user (Nome or "Usuário" if empty)
    /// </summary>
    public string DisplayName()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            return "Usuario";

        // Mostra primeiro e último nome
        string[]? parts = Nome.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts[0] + ' ' + (parts.Length > 1 ? parts[^1] : "");
    }

    /// <summary>
    /// Gets a instance of usuario
    /// </summary>
    public string DisplayFormattedCpf()
    {
        if (string.IsNullOrWhiteSpace(Cpf) || Cpf.Length != 11)
            return Cpf;

        return Convert.ToUInt64(Cpf).ToString(@"000\.000\.000\-00");
    }

    /// <summary>
    /// Gets a instance of usuario
    /// </summary>
    public Usuario GetUsuario()
    {
        return new Usuario
        {
            Id = this.Id,
            Cpf = this.Cpf,
            Nome = this.Nome,
            Email = this.Email,
            Senha = string.Empty,
            Grupo = this.Grupo
        };
    }
}