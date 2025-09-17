using Preventech.Server.Components;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add API controller support
builder.Services.AddControllers();

// Configure PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<OrdemServicoService>();

builder.Services.AddHttpClient<EquipamentoService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8080/");
});

builder.Services.AddHttpClient<UsuarioService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8080/");
});

builder.Services.AddHttpClient<OrdemServicoService>(client =>
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

// Map API controllers
app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
