using Autofac;

namespace Seed.Common.Autofac
{
    public interface IRegistry
    {
        void Register(ContainerBuilder builder);
    }
}