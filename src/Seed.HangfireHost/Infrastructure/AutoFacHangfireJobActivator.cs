using System;
using Autofac;
using Hangfire;
using Hangfire.Annotations;

namespace Seed.HangfireHost.Infrastructure
{
    //
    // PROVENANCE: https://github.com/HangfireIO/Hangfire.Autofac/blob/master/HangFire.Autofac/AutofacJobActivator.cs
    //
    public class AutoFacHangfireJobActivator : JobActivator
    {
        // Constants
        public static readonly object LifetimeScopeTag = "BackgroundJobScope";

        // Instance Variables
        private readonly ILifetimeScope _lifetimeScope;

        private readonly bool _useTaggedLifetimeScope;


        // C'tor
        public AutoFacHangfireJobActivator([NotNull] ILifetimeScope lifetimeScope, bool useTaggedLifetimeScope = true)
        {
            _lifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope));

            _useTaggedLifetimeScope = useTaggedLifetimeScope;
        }


        // JobActivator Overrides
        public override object ActivateJob(Type jobType)
        {
            return _lifetimeScope.Resolve(jobType);
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new AutofacScope(_useTaggedLifetimeScope
                ? _lifetimeScope.BeginLifetimeScope(LifetimeScopeTag)
                : _lifetimeScope.BeginLifetimeScope());
        }


        // Nested Classes
        public class AutofacScope : JobActivatorScope
        {
            private readonly ILifetimeScope _lifetimeScope;

            public AutofacScope(ILifetimeScope lifetimeScope)
            {
                _lifetimeScope = lifetimeScope;
            }

            public override object Resolve(Type type)
            {
                return _lifetimeScope.Resolve(type);
            }

            public override void DisposeScope()
            {
                _lifetimeScope.Dispose();
            }
        }
    }
}