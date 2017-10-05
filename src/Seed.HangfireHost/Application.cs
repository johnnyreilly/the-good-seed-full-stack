using System;
using Autofac;
using DasMulli.Win32.ServiceUtils;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Serilog;
using Seed.HangfireHost.Infrastructure;
using Seed.HangfireHost.Jobs;

namespace Seed.HangfireHost
{
    public class Application : IWin32Service
    {
        private readonly IContainer _container;
        private readonly ILogger _logger;
        private BackgroundJobServer _server;

        // C'tor
        public Application(ILogger logger, IConfigurationRoot config)
        {
            _logger = logger;

            _container = AutofacConfig.Configure(_logger);

            GlobalConfiguration.Configuration.UseSqlServerStorage(
                config.GetConnectionString("SeedToolsDataStoreConnection"));
            GlobalConfiguration.Configuration.UseActivator(new AutoFacHangfireJobActivator(_container));

            RecurringJobs.Configure();
        }

        // IWin32Service Members
        public string ServiceName => "Seed Hangfire Host Service";

        public void Start(string[] startupArguments, ServiceStoppedCallback serviceStoppedCallback)
        {
            try
            {
                _logger.Information("Starting Seed Hangfire Host Service");

                _server = new BackgroundJobServer();

                _logger.Information("Started Seed Hangfire Host Service");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Failed to start Seed Hangfire Host Service");
                throw;
            }
        }

        public void Stop()
        {
            try
            {
                _logger.Information("Stopping Seed Hangfire Host Service");

                _server.Dispose();

                _logger.Information("Stopped Seed Hangfire Host Service");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Failed to stop Seed Hangfire Host Service");
            }
            finally
            {
                _container.Dispose();
            }
        }
    }
}