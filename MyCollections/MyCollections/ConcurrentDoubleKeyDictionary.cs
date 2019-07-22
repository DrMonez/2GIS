using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyCollections
{
    class ConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue> : IConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        private Keys<TKeyId, TKeyName> keys;
        private Dictionary<long, TValue> values;
        IDGenerator idGenerator = new IDGenerator();
        List<RWLock> locks = new List<RWLock>(31);

        public int Count => values.Count;

        public ICollection<TKeyId> IdKeys => keys.IdKeys;

        public ICollection<TKeyName> NameKeys => keys.NameKeys;

        public ICollection<TValue> Values => values.Values;

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool TryAdd(TKeyId id, TKeyName name, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetById(TKeyId id, out Dictionary<TKeyName, TValue> result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetByName(TKeyName name, out Dictionary<TKeyId, TValue> result)
        {
            throw new NotImplementedException();
        }

        public bool TryRemove(TKeyId id, TKeyName name)
        {
            throw new NotImplementedException();
        }

        public ConcurrentDoubleKeyDictionary()
        {
            for (var i = 0; i < locks.Count; i++)
                locks[i] = new RWLock();
        }

        private int GetLockNumber()
        {
            throw new NotImplementedException();
        }
    }

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

        private readonly ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();

        public ReadLockToken ReadLock() => new ReadLockToken(@lock);
        public WriteLockToken WriteLock() => new WriteLockToken(@lock);

        public void Dispose() => @lock.Dispose();
    }

}
