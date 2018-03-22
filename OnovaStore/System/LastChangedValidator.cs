using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using OnovaStore.Helpers;

public static class RefreshTokenMonitor
{
    public static async Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        // Find issued datetime
        var issuedClaim = context.Principal.FindFirst(c => c.Type == JwtRegisteredClaimNames.Iat)?.Value;
        var issuedAt = Convert.ToInt64(issuedClaim).ToUnixEpochDate();

        // Find expiration datetime
        var expiresClaim = context.Principal.FindFirst(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
        var expiresAt = Convert.ToInt64(expiresClaim).ToUnixEpochDate();

        // Calculate how many minutes the token is valid
        var validWindow = (expiresAt - issuedAt).TotalMinutes;

        // Refresh token half way the expiration
        var refreshDateTime = issuedAt.AddMinutes(0.5 * validWindow);

        // Refresh JWT Token if needed
        if (DateTime.UtcNow > refreshDateTime)
        {
            // Get original token from claims
            var jwtToken = context.Principal.FindFirst("jwt")?.Value;

            // Pull ClaimManager from Dependency Injection
            var claimPrincipalManager = context.HttpContext.RequestServices.GetService<IClaimPrincipalManager>();

            // refresh token and claims and expire times
            await claimPrincipalManager.RenewTokenAsync(jwtToken);
        }
    }
}