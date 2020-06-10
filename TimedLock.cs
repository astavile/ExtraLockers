using System;
using System.Threading;
using System.Diagnostics;

namespace NetProtocol.Lockers
{
    /*
      * http://www.interact-sw.co.uk/iangblog/2004/04/26/yetmoretimedlocking
     */

    /// <summary>
    /// Структура для запроса блокировок с таймаутом (на основе Monitor).
    /// Примечание: использовать TimedLock для ситуаций, когда кол-во потоков чтения ~ кол-ву потоков записи.
    /// Если кол-во потоков чтения >> кол-ва потоков записи, то следует использовать ReadWriteLock.
    /// </summary>
    public struct TimedLock : IDisposable
    {
        private const double DefaultTryEnterTimeoutInSeconds = 40;
        private readonly object lockingObject;

        public bool IsLocked { get; private set; }

        public static TimedLock Lock(object lockingObject)
        {
            return Lock(lockingObject, TimeSpan.FromSeconds(DefaultTryEnterTimeoutInSeconds));
        }

        public static TimedLock Lock(object lockingObject, TimeSpan timeout)
        {
            var locker = new TimedLock(lockingObject);

            var isLockTaken = false;
            Monitor.TryEnter(lockingObject, timeout, ref isLockTaken);

            if (!isLockTaken)
                throw new LockTimeoutException();

            locker.IsLocked = isLockTaken;
            return locker;
        }

        private TimedLock(object lockingObject) : this()
        {
            this.lockingObject = lockingObject;
            IsLocked = false;
        }

        public void Dispose()
        {
            try
            {
                if (IsLocked)
                    Monitor.Exit(lockingObject);
            }
            catch (SynchronizationLockException x)
            {
                Log.Error(x.Message);
            }
        }
    }
}