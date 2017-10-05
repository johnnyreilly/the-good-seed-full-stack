using System;
using System.Threading.Tasks;
using AutoMapper;
using Seed.Common.Exceptions;
using Seed.Common.Logging;
using Seed.Data.Entities;
using Seed.Domain.ApiWrappers.Tpd;
using Seed.Domain.ApiWrappers.Tpd.Commands;
using Seed.Domain.ApiWrappers.Tpd.Entities;
using Seed.Domain.Entities;

namespace Seed.Domain.Services
{
    public interface ISecurityService
    {
        Task<LoginResult> LoginAsync(string username, string password);
    }

    public class SecurityService : ISecurityService
    {
        private readonly ILoggingService _loggingService;
        private readonly IMapper _mapper;

        private readonly ITpdAccessTokenStore _tpdAccessTokenStore;

        // Instance Variables
        private readonly ITpdClient _tpdClient;


        // C'tor
        public SecurityService(ITpdClient tpdClient, ITpdAccessTokenStore tpdAccessTokenStore,
            ILoggingService loggingService, IMapper mapper)
        {
            _tpdClient = tpdClient;
            _tpdAccessTokenStore = tpdAccessTokenStore;
            _loggingService = loggingService;
            _mapper = mapper;
        }


        // ISecurityService Members
        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            var command = new LoginCommand(username, password);

            var tpdLoginResult = await _tpdClient.ExecuteAsync(command);

            if (tpdLoginResult.ReturnStatus == LoginStatus.Failed)
            {
                _loggingService.Information(
                    $"User '{username}' failed Tpd login. Error message was '{tpdLoginResult.FailureReason}' ({tpdLoginResult.FailureCode})",
                    parameters: new object[] {tpdLoginResult.FailureCode});

                throw new UnauthorizedApiException(ErrorCodes.LOGIN_FAILED, "Failed to login");
            }

            var tpdAccessToken = new TbsAccessToken(tpdLoginResult.OAuthToken.AccessToken, tpdLoginResult.Gcn, null,
                DateTime.UtcNow.AddSeconds(tpdLoginResult.OAuthToken.ExpiresIn));

            await _tpdAccessTokenStore.StoreTokenAsync(tpdAccessToken);

            var result = _mapper.Map<LoginResult>(tpdLoginResult);

            return result;
        }
    }
}