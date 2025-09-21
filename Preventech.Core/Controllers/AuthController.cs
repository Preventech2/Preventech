using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Preventech.Core.Constants;
using Preventech.Core.DTOs;
using Preventech.Core.Models;

namespace Preventech.Core.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IHttpContextAccessor httpContextAccessor, ILogger<AuthController> logger)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("login")]
        public ApiResponse<string> Login([FromBody] Usuario usuario)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, usuario.Nome ?? string.Empty),
                    new(ClaimTypes.Email, usuario.Email ?? string.Empty),
                    new("Cpf", usuario.Cpf ?? string.Empty)
                };

                foreach (var flag in Enum.GetValues<Perfil>())
                {
                    if (flag != Perfil.NenhumaPermissao && usuario.Perfil.HasValue && usuario.Perfil.Value.HasFlag(flag))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, flag.ToString()));
                    }
                }

                var identity = new ClaimsIdentity(claims, "CustomAuth");

                var token = new JwtSecurityToken(
                    issuer: AuthConstants.JwtIssuer,
                    audience: AuthConstants.JwtAudience,
                    claims: identity.Claims,
                    expires: DateTime.UtcNow.AddHours(AuthConstants.JwtExpiryInHours),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthConstants.JwtSecretKey)),
                        SecurityAlgorithms.HmacSha256)
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                
                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "User authenticated",
                    Data = tokenString
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Erro ao fazer login: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}
