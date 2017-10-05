using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Seed.Domain.Infrastructure.Configuration;

namespace Seed.Api.Infrastructure
{
    public class GcnHeaderSecurityMiddleware
    {
        private readonly SecurityConfiguration _config;

        // Instance Variables
        private readonly RequestDelegate _next;


        // C'tor
        public GcnHeaderSecurityMiddleware(RequestDelegate next, IOptions<SecurityConfiguration> config)
        {
            _next = next;
            _config = config.Value;
        }


        // Middleware Members
        public async Task Invoke(HttpContext context)
        {
            var value = context.Request.Headers[_config.GcnHeaderKey];

            if (!string.IsNullOrWhiteSpace(value))
            {
                var identity = new ClaimsIdentity("GCN-HEADER");

                var claim = new Claim(ClaimTypes.NameIdentifier, value, ClaimValueTypes.String);
                identity.AddClaim(claim);

                context.User = new ClaimsPrincipal(identity);
            }

            await _next.Invoke(context);
        }
    }
}