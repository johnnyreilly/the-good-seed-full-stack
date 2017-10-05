using System;
using System.Linq;
using Autofac.Builder;
using Hangfire.Annotations;

namespace Seed.HangfireHost.Infrastructure
{
    //
    // PROVENANCE: https://github.com/HangfireIO/Hangfire.Autofac/blob/master/HangFire.Autofac/RegistrationExtensions.cs
    //
    public static class AutofacRegistrationExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle>
            InstancePerBackgroundJob<TLimit, TActivatorData, TStyle>(
                [NotNull] this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration)
        {
            return registration.InstancePerBackgroundJob(new object[] { });
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle>
            InstancePerBackgroundJob<TLimit, TActivatorData, TStyle>(
                [NotNull] this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration,
                params object[] lifetimeScopeTags)
        {
            if (registration == null)
                throw new ArgumentNullException(nameof(registration));

            var tags = new[] {AutoFacHangfireJobActivator.LifetimeScopeTag}.Concat(lifetimeScopeTags).ToArray();

            return registration.InstancePerMatchingLifetimeScope(tags);
        }
    }
}