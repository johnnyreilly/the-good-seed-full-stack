using System;
using System.Threading.Tasks;
using Seed.Common.Logging;

namespace Seed.Domain.Services
{
    public interface IStatusService
    {
        Task<string> GetStatusAsync();
    }

    public class StatusService : IStatusService
    {
        // Instance Variables
        private readonly ILoggingService _logger;

        // C'tor
        public StatusService(ILoggingService logger)
        {
            _logger = logger;
        }


        // IStatusService Members
        public Task<string> GetStatusAsync()
        {
            var status = "Up";
            var message = $"Status is '{status}' at {DateTime.UtcNow:HH:mm:ss}";

            _logger
                .ForContext("Status", status)
                .Information(message);

            return Task.FromResult(message);
        }
    }
}