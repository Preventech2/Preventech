using System;
using System.Text.Json;
using Microsoft.JSInterop;
using Preventech.Core.Models;

namespace Preventech.Core.Services;

public class AuthService(IJSRuntime jsRuntime)
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public async Task<bool> Login(Usuario usuario)
    {
        try
        {
            // Serializa o usuário completo automaticamente
            var usuarioJson = JsonSerializer.Serialize(usuario, _jsonOptions);

            var jsCode = $@"
                (async () => {{
                    const Usuario = {usuarioJson};
                    
                    const response = await fetch('/api/auth/login', {{
                        method: 'POST',
                        headers: {{
                            'Content-Type': 'application/json'
                        }},
                        body: JSON.stringify(Usuario),
                        credentials: 'include'
                    }});
                    
                    if (response.ok && response.redirected) {{
                        window.location.href = response.url;
                        return true;
                    }}
                    
                    if (response.ok) {{
                        return true;
                    }}
                    
                    const errorText = await response.text();
                    console.error('Erro no login:', errorText);
                    return false;
                }})()
            ";

            return await _jsRuntime.InvokeAsync<bool>("eval", jsCode);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no login: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Logout()
    {
        try
        {
            var jsCode = @"
                (async () => {
                    const response = await fetch('/api/auth/logout', {
                        method: 'POST',
                        credentials: 'include'
                    });
                    
                    if (response.ok && response.redirected) {
                        console.log('Redirecionando para:', response.url);
                        window.location.href = response.url;
                        return true;
                    }
                    
                    if (response.ok) {
                        window.location.href = '/account/login';
                        return true;
                    }
                    
                    const errorText = await response.text();
                    console.error('Erro no logout:', errorText);
                    return false;
                })()
            ";

            return await _jsRuntime.InvokeAsync<bool>("eval", jsCode);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no logout: {ex.Message}");
            return false;
        }
    }
}