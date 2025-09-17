using Preventech.Server.Components;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.Services;
using Microsoft.EntityFrameworkCore;
using Preventech.Server.SecurityServices;
using Microsoft.AspNetCore.Components.Authorization;
using Quartz;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add API controller support
builder.Services.AddControllers();

// Configure HTTPS redirection
//builder.Services.AddHttpsRedirection(options =>
//{
//    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
//    options.HttpsPort = 7111; // Porta HTTPS definida para suprimir o aviso de segurança
//});

// Add Authorization and Authentication services

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Add scope to the AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationStateService>();
builder.Services.AddScoped<AuthenticationStateService>();

// Configure PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure HttpClient
//builder.Services.AddHttpClient<EquipamentoService>(client =>
//{
//    client.BaseAddress = new Uri("http://localhost:5091/");
//});

builder.Services.AddScoped<OrdemServicoService>();

builder.Services.AddHttpClient<EquipamentoService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5091/");
});

builder.Services.AddHttpClient<UsuarioService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5091/");
});

builder.Services.AddHttpClient<EmailService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5091/");
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

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

// Map API controllers
app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
