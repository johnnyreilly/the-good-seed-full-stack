using System.Security.Claims;
using Seed.Common.Exceptions;

namespace Seed.Api.Infrastructure
{
    public static class ClaimsPrincipal_Extensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedApiException(ErrorCodes.UNAUTHORIZED, "Unauthorized. GCN not set on principal.");

            return userId;
        }
    }
}