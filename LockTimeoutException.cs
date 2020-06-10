using System;

namespace NetProtocol.Lockers
{
    public sealed class LockTimeoutException : ApplicationException
    {
        public LockTimeoutException()
            : base("Timeout waiting for lock")
        {
        }

        public LockTimeoutException(ReadWriteLockMode mode)
            : base(string.Format("Timeout waiting for '{0}' mode", mode.ToString()))
        {
        }
    }
}