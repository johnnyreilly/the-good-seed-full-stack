using System;
using Autofac;

namespace Seed.Api.Tests.Infrastructure
{
    public class ContainerConfigurator : IContainerConfigurator
    {
        // C'tor
        public ContainerConfigurator(Action<ContainerBuilder> configuration)
        {
            Configure = configuration ?? (builder => { });
        }

        // Properties
        public Action<ContainerBuilder> Configure { get; }
    }
}