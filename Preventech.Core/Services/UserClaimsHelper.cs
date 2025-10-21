using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Preventech.Core.Services;

public class UserClaimsHelper
{
    private readonly AuthenticationStateProvider _authStateProvider;

    public UserClaimsHelper(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    /// <summary>
    /// Gets the current user's information from claims
    /// </summary>
    /// <returns>UserInfo object with Name and Cpf, or null if not authenticated</returns>
    public async Task<UserInfo?> GetCurrentUserInfoAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            return null;
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

        var idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var userId = !string.IsNullOrEmpty(idClaim) && int.TryParse(idClaim, out var id) ? id : 0;

        var roles = user.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        return new UserInfo
        {
            Id = userId,
            Name = name,
            Cpf = cpf,
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
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
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
    /// Gets a display name for the user (Name or "Usuário" if empty)
    /// </summary>
    public string DisplayName => !string.IsNullOrEmpty(Name) ? Name : "Usuário";
}