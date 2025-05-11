using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Kabanosi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}