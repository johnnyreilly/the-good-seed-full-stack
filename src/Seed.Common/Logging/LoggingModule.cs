using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Serilog;

namespace Seed.Common.Logging
{
    public class LoggingModule : Module
    {
        private readonly IAuditLogger _auditLogger;
        private readonly ILogger _logger;

        public LoggingModule(ILogger logger, IAuditLogger auditLogger)
        {
            _logger = logger;
            _auditLogger = auditLogger;
        }

        private void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            // In short: if ParameterType is ILoggingService then return new LoggingService<TCaller>(_logger, _auditLogger)
            var resolvedParameter =
                new ResolvedParameter(
                    (p, i) => p.ParameterType == typeof(ILoggingService),
                    (p, i) => Activator.CreateInstance(typeof(LoggingService<>).MakeGenericType(p.Member.DeclaringType),
                        _logger, _auditLogger));

            e.Parameters = e.Parameters.Union(new[] {resolvedParameter});
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry,
            IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }
    }
}