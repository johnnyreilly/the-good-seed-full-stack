using Autofac;
using Seed.Common.Autofac;

namespace Seed.Data.Infrastructure
{
    public class DataRegistry : IRegistry
    {
        public void Register(ContainerBuilder builder)
        {
            builder.Register(c => c.Resolve<SeedToolsDbContext>()).As<ISeedToolsDbContext>();
            builder.RegisterGeneric(typeof(UnitOfWork<>)).As(typeof(IUnitOfWork<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(UnitOfWork<,>)).As(typeof(IUnitOfWork<,>)).InstancePerLifetimeScope();
        }
    }
}