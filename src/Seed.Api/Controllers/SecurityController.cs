using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Seed.Api.Infrastructure;
using Seed.Api.Models.Inputs;
using Seed.Api.Models.ViewModels;
using Seed.Common.Exceptions;
using Seed.Data;
using Seed.Domain.Infrastructure.Configuration;
using Seed.Domain.Services;

namespace Seed.Api.Controllers
{
    public class SecurityController : Controller
    {
        private readonly SecurityConfiguration _config;

        private readonly IMapper _mapper;

        // Instance Variables
        private readonly IUnitOfWork<ISecurityService> _securityServiceUnitOfWork;


        // C'tor
        public SecurityController(IUnitOfWork<ISecurityService> securityServiceUnitOfWork,
            IOptions<SecurityConfiguration> config, IMapper mapper)
        {
            _securityServiceUnitOfWork = securityServiceUnitOfWork;
            _mapper = mapper;
            _config = config.Value;
        }


        // Members
        [HttpPost]
        [Route("api/token")]
        public async Task<IActionResult> Post([FromBody] LoginInput loginInput)
        {
            var loginResult = await _securityServiceUnitOfWork.ExecuteAsync(async service =>
            {
                try
                {
                    return await service.LoginAsync(loginInput.Username, loginInput.Password);
                }
                catch (UnauthorizedApiException ex)
                {
                    throw new UnloggedUnitOfWorkException(ex);
                }
            });

            var viewModel = _mapper.Map<LoginResultView>(loginResult);

            viewModel.Cookie.Path = _config.Cookie?.Path;
            viewModel.Cookie.Expires = DateTime.UtcNow.AddSeconds(_config.Cookie?.ExpiryInSeconds ?? 60 * 60);
            viewModel.Cookie.IsSecure = _config.Cookie?.IsSecure ?? false;

            return Ok(viewModel);
        }

        [Authorize]
        [HttpGet]
        [Route("api/users/me")]
        public IActionResult GetMe()
        {
            return Ok(new
            {
                UserId = User.GetUserId()
            });
        }
    }
}