using System;
using System.Threading;

namespace MyCollections
{
    internal class RWLock : IDisposable
    {
        public struct WriteLockToken : IDisposable
        {
            private readonly ReaderWriterLockSlim @lock;
            public WriteLockToken(ReaderWriterLockSlim @lock)
            {
                this.@lock = @lock;
                @lock.EnterWriteLock();
            }
            public void Dispose() => @lock.ExitWriteLock();
        }

        public struct ReadLockToken : IDisposable
        {
            private readonly ReaderWriterLockSlim @lock;
            public ReadLockToken(ReaderWriterLockSlim @lock)
            {
                this.@lock = @lock;
                @lock.EnterReadLock();
            }
            public void Dispose() => @lock.ExitReadLock();
        }

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public ReadLockToken ReadLock() => new ReadLockToken(_lock);
        public WriteLockToken WriteLock() => new WriteLockToken(_lock);

        public void Dispose() => _lock.Dispose();
    }
}
