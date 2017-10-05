using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Seed.Common.Exceptions;

namespace Seed.Web.Controllers
{
    public class SpaController : Controller
    {
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

        public IActionResult FourOhFour()
        {
            throw new NotFoundApiException(ErrorCodes.INVALID_ROUTE,
                $"No route mapped to path '{Request.GetEncodedUrl()}'");
        }
    }
}