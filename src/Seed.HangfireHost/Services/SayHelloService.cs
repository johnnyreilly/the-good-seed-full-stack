using Serilog;

namespace Seed.HangfireHost.Services
{
    public class SayHelloService : ISayHelloService
    {
        private readonly ILogger _logger;

        public SayHelloService(ILogger logger)
        {
            _logger = logger;
        }

        public void SayHello()
        {
            _logger.Information("Hello, World!");
        }
    }
}