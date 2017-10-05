using System;
using System.IO;
using Destructurama;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Seed.Common.Logging
{
    public static class LoggingConfig
    {
        public static ILogger Configure(IConfigurationRoot configuration)
        {
            var serilogConfig = configuration.GetSection("Serilog");
            var loggingConfig = configuration.GetSection("Logging");

            var logFilename = serilogConfig.GetValue<string>("LogFilename")
                .Replace("${basedir}", Directory.GetCurrentDirectory());
            var seqEnabled = serilogConfig.GetValue<bool>("SeqEnabled");
            var seqApiKey = serilogConfig.GetValue<string>("SeqApiKey");
            var seqUrl = serilogConfig.GetValue<string>("SeqUrl");

            var loggerConfiguration =
                new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    //.Enrich.With(new LogEnricher(() => Thread..Identity?.GetUserId()))
                    .Destructure.UsingAttributes()
                    .WriteTo.RollingFile(pathFormat: logFilename, fileSizeLimitBytes: null, retainedFileCountLimit: 100,
                        formatter: new JsonFormatter());

            if (seqEnabled)
                loggerConfiguration = loggerConfiguration.WriteTo.Seq(seqUrl, apiKey: seqApiKey);

            foreach (var logLevel in loggingConfig.GetSection("LogLevel").GetChildren())
            {
                var logEventLevel = GetLogEventLevel(logLevel.Value);

                if (logLevel.Key == "Default")
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Is(logEventLevel);
                else
                    loggerConfiguration = loggerConfiguration.MinimumLevel.Override(logLevel.Key, logEventLevel);
            }

            return loggerConfiguration.CreateLogger();
        }

        private static LogEventLevel GetLogEventLevel(string value)
        {
            return (LogEventLevel) Enum.Parse(typeof(LogEventLevel), value);
        }
    }
}