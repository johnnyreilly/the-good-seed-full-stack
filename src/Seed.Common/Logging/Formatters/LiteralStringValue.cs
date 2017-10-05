using System;
using System.IO;
using Serilog.Events;

namespace Seed.Common.Logging.Formatters
{
    public class LiteralStringValue : LogEventPropertyValue
    {
        // Instance Variables
        private readonly string _value;


        // C'tor
        public LiteralStringValue(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _value = value;
        }


        // LogEventPropertyValue Overrides
        public override void Render(TextWriter output, string format = null, IFormatProvider formatProvider = null)
        {
            var toRender = _value;

            switch (format)
            {
                case "u":
                    toRender = _value.ToUpperInvariant();
                    break;
                case "w":
                    toRender = _value.ToLowerInvariant();
                    break;
            }

            output.Write(toRender);
        }


        // System.Object Overrides
        public override bool Equals(object obj)
        {
            var sv = obj as LiteralStringValue;
            return sv != null && Equals(_value, sv._value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}