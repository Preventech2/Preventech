using PrevenTech.Components;
using static Microsoft.AspNetCore.Http.StatusCodes;
using System.Security.Cryptography.X509Certificates;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials()
            .WithOrigins(
            "https://localhost:5001/",
            "https://0.0.0.0:5000");
    })
);

if (!builder.Environment.IsDevelopment())
{
    // openssl req -newkey rsa:2048 -new -nodes -x509 -days 3650 -keyout key.pem -out cert.pem
    var certificate = X509Certificate2.CreateFromPemFile("cert.pem", "key.pem");
    builder.WebHost.ConfigureKestrel((context, options) =>
    {
        options.ConfigureHttpsDefaults(adapterOptions =>
        {
            adapterOptions.ServerCertificate = certificate;
        });
        options.Listen(IPAddress.Any, 5001, listenOptions =>
        {
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2AndHttp3;
            listenOptions.UseHttps();
        });
    });
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
});

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = Status308PermanentRedirect;
    options.HttpsPort = 5001;
});

var app = builder.Build();
app.MapControllers();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UsePathBase("/out/wwwroot/");
app.UseHttpsRedirection();

app.UseAntiforgery();
app.UseAuthorization();
app.MapStaticAssets();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.Run();
