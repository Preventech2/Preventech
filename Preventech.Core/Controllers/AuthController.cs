using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Preventech.Core.Constants;
using Preventech.Core.Models;

namespace Preventech.Core.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("Dados do usuário inválidos");
            }
            
            var perfil = usuario.Grupo?.Permissoes ?? Perfil.NenhumaPermissao;
            var identity = new ClaimsIdentity(AuthConstants.CookieName);

            identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nome ?? string.Empty));
            identity.AddClaim(new Claim("Cpf", usuario.Cpf ?? string.Empty));
            identity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty));
            identity.AddClaim(new Claim("Grupo", usuario.Grupo?.Nome ?? string.Empty));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString() ?? string.Empty));

            foreach (var flag in Enum.GetValues<Perfil>())
            {
                if (flag == Perfil.NenhumaPermissao || flag == Perfil.All) continue;
                if (perfil.HasFlag(flag))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, flag.ToString()));
                }
            }

            identity.AddClaim(new Claim(ClaimTypes.Sid, AuthConstants.Sid));
            identity.AddClaim(new Claim(ClaimTypes.Expiration,
                DateTime.Now.AddSeconds(AuthConstants.CookieExpiry).ToString()
            ));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(
                AuthConstants.CookieName,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddSeconds(AuthConstants.CookieExpiry)
                }
            );
            return Redirect("/");
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthConstants.CookieName);
            return Redirect("/");
        }
    }
}
