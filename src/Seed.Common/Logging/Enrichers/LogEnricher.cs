using System;
using Serilog.Core;
using Serilog.Events;

namespace Seed.Common.Logging.Enrichers
{
    public class LogEnricher : ILogEventEnricher
    {
        private readonly Func<string> _getUserId;

        public LogEnricher(Func<string> getUserId)
        {
            _getUserId = getUserId;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var userId = _getUserId();

            if (userId != null)
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserId", userId, true));

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Machine", Environment.MachineName, true));
        }
    }
}