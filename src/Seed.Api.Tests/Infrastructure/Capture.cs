using System;
using System.Collections.Generic;
using FakeItEasy;

namespace Seed.Api.Tests.Infrastructure
{
    // http://thorarin.net/blog/post/2014/09/18/capturing-method-arguments-on-your-fakes-using-fakeiteasy.aspx

    public sealed class Capture<T>
    {
        private readonly List<T> _values = new List<T>();
        private bool _pendingConfiguration = true;

        public T Value
        {
            get
            {
                if (_values.Count == 0)
                    throw new InvalidOperationException("No values have been captured.");

                if (_values.Count > 1)
                    throw new InvalidOperationException("Multiple values were captured. Use Values property instead.");

                return _values[0];
            }
        }

        public IReadOnlyList<T> Values => _values.AsReadOnly();

        public bool HasValues => _values.Count > 0;

        private void CaptureValue(T value)
        {
            _values.Add(value);
        }

        public override string ToString()
        {
            if (_values.Count == 0) return "No captured values";
            if (_values.Count == 1) return Value.ToString();
            return string.Format("{0} captured values", Values.Count);
        }

        public static implicit operator T(Capture<T> capture)
        {
            if (!capture._pendingConfiguration)
                throw new InvalidOperationException("Capture can only be used to configure a single call." +
                                                    " If you're trying to access the captured value, use the Value property instead of relying on an implicit conversion.");

            A<T>.That.Matches(input =>
            {
                capture.CaptureValue(input);
                return true;
            }, "Captured parameter " + typeof(T).FullName);
            capture._pendingConfiguration = false;

            return default(T);
        }
    }
}