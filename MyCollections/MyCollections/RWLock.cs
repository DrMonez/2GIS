using System;
using System.Threading;

namespace MyCollections
{
    internal class RWLock : IDisposable
    {
        public struct WriteLockToken : IDisposable
        {
            private readonly ReaderWriterLockSlim _lock;
            public WriteLockToken(ReaderWriterLockSlim writeLock)
            {
                this._lock = writeLock;
                writeLock.EnterWriteLock();
            }
            public void Dispose() => _lock.ExitWriteLock();
        }

        public struct ReadLockToken : IDisposable
        {
            private readonly ReaderWriterLockSlim _lock;
            public ReadLockToken(ReaderWriterLockSlim readLock)
            {
                this._lock = readLock;
                readLock.EnterReadLock();
            }
            public void Dispose() => _lock.ExitReadLock();
        }

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public ReadLockToken ReadLock() => new ReadLockToken(_lock);
        public WriteLockToken WriteLock() => new WriteLockToken(_lock);

        public void Dispose() => _lock.Dispose();
    }
}
