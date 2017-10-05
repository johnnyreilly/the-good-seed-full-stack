using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Seed.Domain.Services;

namespace Seed.Api.Controllers
{
    public class StatusController : Controller
    {
        // Instance Variables
        private readonly IStatusService _statusService;


        // C'tor
        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }


        // Members
        [HttpGet]
        [Route("api/status")]
        public async Task<IActionResult> Get()
        {
            var status = await _statusService.GetStatusAsync();

            return Ok(status);
        }
    }
}