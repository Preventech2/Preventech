using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace Preventech.Core.Constants;

public class AuthConstants
{
    /// <summary>
    /// Para criação do token JWT 
    /// </summary>
    public const string JwtIssuer = "Preventech";
    public const string JwtAudience = "PreventechUsers";
    public const int JwtExpiryInHours = 1;
    public const string JwtSecretKey = "nkNXNa5malFk9BSqwTlbefBkimaTNlPa0UmHL5MyhtA=";

    /// <summary>
    /// Configurações do cookie de autenticação
    /// </summary>
    public const string Sid = "555"; // Valor fixo para demonstração, substituir por lógica real de sessão
    public const string CookieName = "AuthToken";
    public const bool CookieHttpOnly = true;
    public const SameSiteMode CookieSameSite = SameSiteMode.Strict; // Options: Lax, Strict,
    public const int CookieExpiry = 10; // Expiração em segundos (1 hora)

}
