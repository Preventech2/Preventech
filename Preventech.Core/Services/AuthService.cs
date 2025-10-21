using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Preventech.Core.Models;

namespace Preventech.Core.Services;

public class AuthService
{
    private readonly IJSRuntime _jsRuntime;

    public AuthService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> Login(Usuario usuario)
    {
        try
        {
            // Send Usuario as JSON to match controller expectation
            var jsCode = $@"
                (async () => {{
                    const usuarioData = {{
                        Id: {usuario.Id},
                        Nome: '{usuario.Nome}',
                        Cpf: '{usuario.Cpf}',
                        Perfil: {(int)(usuario.Perfil ?? Perfil.NenhumaPermissao)}
                    }};
                    
                    const response = await fetch('/api/auth/login', {{
                        method: 'POST',
                        headers: {{
                            'Content-Type': 'application/json'
                        }},
                        body: JSON.stringify(usuarioData),
                        credentials: 'include'
                    }});
                    
                    if (response.ok && response.redirected) {{
                        window.location.href = response.url;
                        return true;
                    }}
                    return response.ok;
                }})()
            ";
            
            return await _jsRuntime.InvokeAsync<bool>("eval", jsCode);
        }
        catch
        {
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
                        window.location.href = response.url;
                        return true;
                    }
                    return response.ok;
                })()
            ";
            
            return await _jsRuntime.InvokeAsync<bool>("eval", jsCode);
        }
        catch
        {
            return false;
        }
    }
}