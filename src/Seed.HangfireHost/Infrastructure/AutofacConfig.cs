using Autofac;
using Serilog;
using Seed.HangfireHost.Services;

namespace Seed.HangfireHost.Infrastructure
{
    public static class AutofacConfig
    {
        public static IContainer Configure(ILogger logger)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(logger).As<ILogger>();
            builder.RegisterType<SayHelloService>().As<ISayHelloService>().InstancePerBackgroundJob();

            var container = builder.Build();

            return container;
        }
    }
}