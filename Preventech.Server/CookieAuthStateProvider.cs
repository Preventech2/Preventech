using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Preventech.Core.Constants;

namespace Preventech.Server;

public class CookieAuthStateProvider : RevalidatingServerAuthenticationStateProvider
{
    public CookieAuthStateProvider(ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(AuthConstants.CookieExpiry);

    protected override Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        var result = false;
        var user = authenticationState?.User;

        if (user?.Identity?.IsAuthenticated ?? false)
        {
            var expiry = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Expiration)?.Value ?? "";

            if (!string.IsNullOrEmpty(expiry))
            {
                if (DateTime.TryParse(expiry, out var exp))
                {
                    if (exp > DateTime.Now)
                    {
                        result = true;
                    }
                }
            }
        }

        return Task.FromResult(result);
    } 
}
