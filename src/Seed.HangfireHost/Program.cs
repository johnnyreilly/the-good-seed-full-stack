using System;
using DasMulli.Win32.ServiceUtils;
using Microsoft.Extensions.Configuration;
using Seed.HangfireHost.Infrastructure;

namespace Seed.HangfireHost
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var logger = Logging.Configure(config);
            try
            {
                var application = new Application(logger, config);
                var serviceHost = new Win32ServiceHost(application);
                serviceHost.Run();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Failed to run Seed Hangfire service host ");
                throw;
            }
        }
    }
}