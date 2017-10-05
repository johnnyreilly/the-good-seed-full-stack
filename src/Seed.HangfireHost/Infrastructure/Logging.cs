using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.RollingFile;

namespace Seed.HangfireHost.Infrastructure
{
    public static class Logging
    {
        public static ILogger Configure(IConfigurationRoot config)
        {
            var logFilename = config["Serilog:LogFilename"];
            var seqEnabled = bool.Parse(config["Serilog:SeqEnabled"]);
            var seqApiKey = config["Serilog:SeqApiKey"];
            var seqUrl = config["Serilog:SeqUrl"];
            var directory = logFilename.Replace("${basedir}", Directory.GetCurrentDirectory());

            var logSink = new RollingFileSink(directory, new JsonFormatter(), null, 100);

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Sink(logSink);

            if (seqEnabled)
                return loggerConfiguration
                    .WriteTo.Seq(seqUrl, apiKey: seqApiKey)
                    .CreateLogger();

            return loggerConfiguration.CreateLogger();
        }
    }
}