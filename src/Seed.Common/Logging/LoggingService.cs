using System;
using System.Runtime.CompilerServices;
using Serilog;

namespace Seed.Common.Logging
{
    public class LoggingService<T> : ILoggingService
    {
        // Constants
        public const string MemberNamePropertyName = "MemberName";

        public const string AuditingPropertyName = "Auditing";
        private readonly ILogger _auditLogger;

        // Instance Variables
        private readonly ILogger _logger;


        // C'tors
        public LoggingService(ILogger logger, IAuditLogger auditLogger)
        {
            _logger = logger.ForContext(typeof(T));
            _auditLogger = auditLogger.ForContext(typeof(T));
        }

        internal LoggingService(ILogger logger, ILogger auditLogger)
        {
            _logger = logger;
            _auditLogger = auditLogger;
        }


        // ILoggingService Members
        public ILoggingService ForContext(Type type)
        {
            return new LoggingService<T>(_logger.ForContext(type), _auditLogger.ForContext(type));
        }

        public ILoggingService ForContext(string key, object value, bool isPii = false)
        {
            return new LoggingService<T>(_logger.ForContext(key, value),
                isPii ? _auditLogger : _auditLogger.ForContext(key, value));
        }

        public ILoggingService Debug(string message, string memberName = "", params object[] parameters)
        {
            _logger
                .ForContext(MemberNamePropertyName, memberName)
                .Debug(message, parameters);

            return this;
        }

        public ILoggingService Information(string message, [CallerMemberName] string memberName = "",
            params object[] parameters)
        {
            _logger
                .ForContext(MemberNamePropertyName, memberName)
                .Information(message, parameters);

            return this;
        }

        public ILoggingService Error(string message, [CallerMemberName] string memberName = "",
            params object[] parameters)
        {
            _logger
                .ForContext(MemberNamePropertyName, memberName)
                .Error(message, parameters);

            return this;
        }

        public ILoggingService Error(Exception ex, string message, [CallerMemberName] string memberName = "",
            params object[] parameters)
        {
            _logger
                .ForContext(MemberNamePropertyName, memberName)
                .Error(ex, message, parameters);

            return this;
        }

        public ILoggingService Warning(string message, string memberName = "", params object[] parameters)
        {
            _logger
                .ForContext(MemberNamePropertyName, memberName)
                .Warning(message, parameters);

            return this;
        }

        public ILoggingService Warning(Exception ex, string message, string memberName = "", params object[] parameters)
        {
            _logger
                .ForContext(MemberNamePropertyName, memberName)
                .Warning(ex, message, parameters);

            return this;
        }

        public ILoggingService AuditInformation(string message, string memberName = "", params object[] parameters)
        {
            _auditLogger
                .ForContext(MemberNamePropertyName, memberName)
                .Information(message, parameters);
            _logger
                .ForContext(MemberNamePropertyName, memberName)
                .ForContext(AuditingPropertyName, "true")
                .Information(message, parameters);

            return this;
        }

        public ILoggingService AuditError(string message, string memberName = "", params object[] parameters)
        {
            _auditLogger
                .ForContext(MemberNamePropertyName, memberName)
                .Error(message, parameters);
            _logger
                .ForContext(MemberNamePropertyName, memberName)
                .ForContext(AuditingPropertyName, "true")
                .Error(message, parameters);

            return this;
        }

        public ILoggingService AuditError(Exception ex, string message, string memberName = "",
            params object[] parameters)
        {
            _auditLogger
                .ForContext(MemberNamePropertyName, memberName)
                .Error(ex, message, parameters);
            _logger
                .ForContext(MemberNamePropertyName, memberName)
                .ForContext(AuditingPropertyName, "true")
                .Error(ex, message, parameters);

            return this;
        }
    }
}