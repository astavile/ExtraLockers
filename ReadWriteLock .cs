using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetProtocol.Lockers
{
    public struct ReadWriteLock : IDisposable
    {
        private ReadWriteLock(ReaderWriterLockSlim locker, ReadWriteLockMode mode) : this()
        {
            this.locker = locker;
            this.mode = mode;
        }

        private const int DefaultTimeoutInMs = 20000;

        private readonly ReadWriteLockMode mode;
        private readonly ReaderWriterLockSlim locker;

        public static ReadWriteLock Lock(ReaderWriterLockSlim locker, ReadWriteLockMode mode)
        {
            var readWriteLocker = new ReadWriteLock(locker, mode);

            switch (mode)
            {
                case ReadWriteLockMode.Read:
                    if (!locker.TryEnterReadLock(DefaultTimeoutInMs))
                        throw new LockTimeoutException(mode);
                    break;
                case ReadWriteLockMode.Write:
                    if (!locker.TryEnterWriteLock(DefaultTimeoutInMs))
                        throw new LockTimeoutException(mode);
                    break;
                case ReadWriteLockMode.UpgradeableRead:
                    if (!locker.TryEnterUpgradeableReadLock(DefaultTimeoutInMs))
                        throw new LockTimeoutException(mode);
                    break;
            }

            return readWriteLocker;
        }

        public void Dispose()
        {
            if (locker == null)
                return;

            switch (mode)
            {
                case ReadWriteLockMode.Read:
                    locker.ExitReadLock();
                    break;
                case ReadWriteLockMode.Write:
                    locker.ExitWriteLock();
                    break;
                case ReadWriteLockMode.UpgradeableRead:
                    locker.ExitUpgradeableReadLock();
                    break;
            }
        }
    }
}
