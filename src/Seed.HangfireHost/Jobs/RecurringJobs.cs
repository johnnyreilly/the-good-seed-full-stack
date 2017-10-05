using System;
using System.Linq.Expressions;
using Hangfire;
using Seed.HangfireHost.Services;

namespace Seed.HangfireHost.Jobs
{
    public static class RecurringJobs
    {
        public static void Configure()
        {
            AddOrUpdateJob<ISayHelloService>("say-hello", service => service.SayHello());
        }

        private static void AddOrUpdateJob<T>(string jobName, Expression<Action<T>> methodCall)
        {
            var cronExpression = "* 8-18 * * MON-FRI";

            RecurringJob.AddOrUpdate(jobName, methodCall, cronExpression);
        }
    }
}