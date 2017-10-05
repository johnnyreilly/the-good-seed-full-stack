using Autofac;
using Seed.Common.Autofac;
using Seed.Domain.ApiWrappers.Tpd;
using Seed.Domain.Services;

namespace Seed.Domain.Infrastructure
{
    public class DomainRegistry : IRegistry
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<SecurityService>().As<ISecurityService>();
            builder.RegisterType<StatusService>().As<IStatusService>();
            builder.RegisterType<TpdAccessTokenStore>().As<ITpdAccessTokenStore>();
            builder.RegisterType<TpdClient>().As<ITpdClient>();
        }
    }
}