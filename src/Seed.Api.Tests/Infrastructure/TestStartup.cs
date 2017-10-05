using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Seed.Data;
using Seed.Web;

namespace Seed.Api.Tests.Infrastructure
{
    public class TestStartup : Startup
    {
        // Instance Variables
        private readonly IContainerConfigurator _containerConfigurator;

        private readonly string _uniqueDbName;

        // C'tor
        public TestStartup(IHostingEnvironment env, IContainerConfigurator containerConfigurator)
            : base(env)
        {
            _containerConfigurator = containerConfigurator;
            _uniqueDbName = Guid.NewGuid().ToString();
        }

        // Startup Overrides
        protected override void ConfigureDatabase(IServiceCollection services)
        {
            services
                .AddDbContext<SeedToolsDbContext>(opt =>
                {
                    opt.UseInMemoryDatabase(_uniqueDbName);
                    opt.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });
        }

        protected override ILogger ConfigureLogging(IConfigurationRoot configuration)
        {
            var config = new LoggerConfiguration()
                .WriteTo.ColoredConsole();

            return config.CreateLogger();
        }

        protected override void ConfigureContainer(ContainerBuilder builder, IServiceCollection services)
        {
            base.ConfigureContainer(builder, services);

            _containerConfigurator?.Configure(builder);
        }

        protected override void ConfigureHangfireServer(IApplicationBuilder app)
        {
            // No-op - we don't want jobs written to the hangfire tables
        }
    }
}