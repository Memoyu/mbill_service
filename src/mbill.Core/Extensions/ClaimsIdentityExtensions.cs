﻿namespace Mbill.Core.Extensions;

public static class ClaimsIdentityExtensions
{
    public static int? FindUserId(this ClaimsPrincipal principal)
    {
        Claim userIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdOrNull == null || string.IsNullOrWhiteSpace(userIdOrNull.Value))
        {
            return null;
        }

        return int.Parse(userIdOrNull.Value);
    }

    public static bool? IsAdmin(this ClaimsPrincipal principal)
    {
        Claim isAdminOrNull = principal.Claims?.FirstOrDefault(c => c.Type == CoreClaimTypes.IsAdmin);
        if (isAdminOrNull == null || string.IsNullOrWhiteSpace(isAdminOrNull.Value))
        {
            return null;
        }
        return bool.Parse(isAdminOrNull.Value);
    }

    public static string FindUserName(this ClaimsPrincipal principal)
    {
        Claim userNameOrNull = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name);

        return userNameOrNull?.Value;
    }

}
