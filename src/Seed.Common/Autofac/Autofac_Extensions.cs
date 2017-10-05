using Autofac;

namespace Seed.Common.Autofac
{
    public static class Autofac_Extensions
    {
        public static void IncludeRegistry<TRegistry>(this ContainerBuilder container)
            where TRegistry : IRegistry, new()
        {
            var registry = new TRegistry();

            registry.Register(container);
        }
    }
}