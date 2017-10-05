using System;

namespace Seed.Data
{
    public class UnloggedUnitOfWorkException : Exception
    {
        public UnloggedUnitOfWorkException(Exception wrappedException)
        {
            WrappedException = wrappedException;
        }

        public Exception WrappedException { get; }
    }
}