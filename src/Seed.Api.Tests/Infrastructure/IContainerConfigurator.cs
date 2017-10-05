using System;
using Autofac;

namespace Seed.Api.Tests.Infrastructure
{
    public interface IContainerConfigurator
    {
        Action<ContainerBuilder> Configure { get; }
    }
}