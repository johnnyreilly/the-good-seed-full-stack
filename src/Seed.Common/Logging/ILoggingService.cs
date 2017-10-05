using System;
using System.Runtime.CompilerServices;

namespace Seed.Common.Logging
{
    public interface ILoggingService
    {
        ILoggingService ForContext(Type type);

        ILoggingService ForContext(string key, object value, bool isPii = false);

        ILoggingService Debug(string message, [CallerMemberName] string memberName = "", params object[] parameters);

        ILoggingService Information(string message, [CallerMemberName] string memberName = "",
            params object[] parameters);

        ILoggingService Error(string message, [CallerMemberName] string memberName = "", params object[] parameters);

        ILoggingService Error(Exception ex, string message, [CallerMemberName] string memberName = "",
            params object[] parameters);

        ILoggingService Warning(string message, [CallerMemberName] string memberName = "", params object[] parameters);

        ILoggingService Warning(Exception ex, string message, [CallerMemberName] string memberName = "",
            params object[] parameters);

        ILoggingService AuditInformation(string message, [CallerMemberName] string memberName = "",
            params object[] parameters);

        ILoggingService AuditError(string message, [CallerMemberName] string memberName = "",
            params object[] parameters);

        ILoggingService AuditError(Exception ex, string message, [CallerMemberName] string memberName = "",
            params object[] parameters);
    }
}