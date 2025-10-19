using Preventech.Server.Components;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Quartz;
using Preventech.Core.Constants;
using Preventech.Server;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add API controller support
builder.Services.AddControllers();


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthStateProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserClaimsHelper>();

// Header service para gerenciar títulos das páginas
builder.Services.AddSingleton<HeaderService>();

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = AuthConstants.CookieName;
})
.AddCookie(AuthConstants.CookieName, o =>
{
    o.LoginPath = "/login";
    o.LogoutPath = "/logout";
    o.AccessDeniedPath = "/login";
    o.Cookie.Name = AuthConstants.CookieName;
    o.Cookie.SameSite = AuthConstants.CookieSameSite;
    o.ExpireTimeSpan = TimeSpan.FromSeconds(AuthConstants.CookieExpiry);
    o.SlidingExpiration = false; // Define se o cookie deve ser renovado automaticamente
    o.Events = new CookieAuthenticationEvents
    {
        OnValidatePrincipal = ctx =>
        {
            if (ctx.Principal?.Identity?.IsAuthenticated ?? false)
            {
                var Claims = ctx.Principal.Claims;

                // Verifica se o cookie expirou
                var expirationClaim = Claims.FirstOrDefault(c => c.Type == ClaimTypes.Expiration)?.Value;
                if (!string.IsNullOrEmpty(expirationClaim) && DateTime.TryParse(expirationClaim, out var expiration))
                {
                    if (DateTime.Now > expiration)
                    {
                        ctx.RejectPrincipal();
                        return ctx.HttpContext.SignOutAsync(AuthConstants.CookieName);
                    }
                }

                if (Claims == null)
                {
                    ctx.RejectPrincipal();
                    return ctx.HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
                }
                else
                {
                    var sid = Claims.Where(c => c.Type == ClaimTypes.Sid).FirstOrDefault()?.Value ?? "";
                    if (sid != "555")
                    {
                        ctx.RejectPrincipal();
                        return ctx.HttpContext.SignOutAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                }
            }
            return Task.CompletedTask;
        }
    };
});

// Configure PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/var/local"))
    .SetApplicationName("preventech")
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA512
    });

Uri base_uri = new("http://localhost:8080/");

builder.Services.AddScoped<OrdemServicoService>();

builder.Services.AddHttpClient<EquipamentoService>(client =>
{
    client.BaseAddress = base_uri;
});

builder.Services.AddHttpClient<UsuarioService>(client =>
{
    client.BaseAddress = base_uri;
});

builder.Services.AddScoped<OrdemServicoService>();
builder.Services.AddHttpClient<OrdemServicoService>(client =>
{
    client.BaseAddress = base_uri;
});

builder.Services.AddHttpClient<LocalizacaoService>(client =>
{
    client.BaseAddress = base_uri;
});

builder.Services.AddHttpClient<GrupoPerfilService>(client =>
{
    client.BaseAddress = base_uri;
});

builder.Services.AddHttpClient<EmailService>(client =>
{
    client.BaseAddress = base_uri;
});

builder.Services.AddHttpClient<LocalizacaoService>(client =>
{
    client.BaseAddress = base_uri;
});

builder.Services.AddScoped<DocumentoService>();
builder.Services.AddHttpClient<DocumentoService>(client =>
{
    client.BaseAddress = base_uri;
});

builder.Services.AddHttpClient<RelatorioGeneratorService>(client =>
{
    client.BaseAddress = base_uri;
});

builder.Services.AddHttpClient<HabilidadeSistemaService>(client =>
{
    client.BaseAddress = base_uri;
});

builder.Services.AddHttpClient<HabilidadeService>(client =>
{
    client.BaseAddress = base_uri;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// Map API controllers
app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// faz cache de arquivos css -> menos requisições
app.Use(async (context, next) =>
{
    string path = context.Request.Path;

    if (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".png"))
    {
        var tempo = (int)TimeSpan.FromDays(7).TotalSeconds;
        context.Response.Headers.Append("Cache-Control", $"max-age={tempo}");
    }
    else
    {
        context.Response.Headers.Append("Cache-Control", "no-cache");
        context.Response.Headers.Append("Cache-Control", "private, no-store");
    }
    await next();
});

app.Run();
