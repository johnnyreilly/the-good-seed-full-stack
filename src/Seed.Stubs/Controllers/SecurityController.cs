using Microsoft.AspNetCore.Mvc;
using Seed.Stubs.Entities;

namespace Seed.Stubs.Controllers
{
    public class SecurityController : Controller
    {
        [HttpPost]
        [Route("api/Login")]
        public IActionResult Login()
        {
            return Ok(
                new
                {
                    CookieDomain = "localhost",
                    CookieName = "SSODEV",
                    CookieValue = "1234",
                    FailureCode = 0,
                    FailureReason = "",
                    Gcn = "12345",
                    ReturnStatus = 0,
                    OAuthToken = new
                    {
                        access_token = "dummy",
                        token_type = "grant",
                        expires_in = 84600
                    }
                }
            );
        }

        [Route("api/ChangeLegalEntity")]
        [HttpPost]
        public OAuthToken ChangeLegalEntity([FromBody] ChangeLegalEntityRequest request)
        {
            return new OAuthToken
            {
                access_token = "dummy",
                token_type = "grant",
                expires_in = 84600
            };
        }
    }
}