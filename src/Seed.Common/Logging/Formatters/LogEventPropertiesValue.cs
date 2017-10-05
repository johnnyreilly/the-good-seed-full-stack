using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Events;

namespace Seed.Common.Logging.Formatters
{
    public class LogEventPropertiesValue : LogEventPropertyValue
    {
        private readonly HashSet<string> _ignoreTokens;

        // Instance Variables
        private readonly IReadOnlyDictionary<string, LogEventPropertyValue> _properties;


        // C'tor
        public LogEventPropertiesValue(IReadOnlyDictionary<string, LogEventPropertyValue> properties,
            HashSet<string> ignoreTokens)
        {
            _properties = properties;
            _ignoreTokens = ignoreTokens;
        }


        // LogEventPropertyValue Overrides
        public override void Render(TextWriter output, string format = null, IFormatProvider formatProvider = null)
        {
            output.Write(string.Join(" ",
                _properties.Where(m => !_ignoreTokens.Contains(m.Key)).Select(m => $"{m.Key}: {m.Value};")));
        }
    }
}