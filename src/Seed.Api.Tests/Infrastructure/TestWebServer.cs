using System;
using System.Net.Http;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Seed.Data;

namespace Seed.Api.Tests.Infrastructure
{
    public class TestWebServer : IDisposable
    {
        // Instance variables
        private readonly TestServer _server;


        // C'tor
        public TestWebServer(IContainerConfigurator containerConfigurator = null)
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(containerConfigurator ?? new ContainerConfigurator(null));
                })
                .UseStartup<TestStartup>();

            _server = new TestServer(builder);
        }

        public TestWebServer(Action<ContainerBuilder> containerConfiguration)
            : this(new ContainerConfigurator(containerConfiguration))
        {
        }

        public SeedToolsDbContext Database => _server.Host.Services.GetService<SeedToolsDbContext>();

        public IServiceProvider ServiceProvider => _server.Host.Services;


        // IDisposable Members
        public void Dispose()
        {
            _server?.Dispose();
        }


        // Public Members
        public HttpClient GetClient(string userId)
        {
            var client = _server.CreateClient();
            client.DefaultRequestHeaders.Add("GCN", userId);

            return client;
        }
    }
}