using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyCollections
{
    public class ConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue> : IConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        //private readonly int threadsCount = 31;
        private Keys<TKeyId, TKeyName> keys;
        private Dictionary<long, TValue> values;
        IDGenerator idGenerator = new IDGenerator();
        RWLock[] locks = new RWLock[31];
        RWLock globalLocker = new RWLock();

        public int Count
        {
            get
            {
                using (globalLocker.ReadLock())
                    return values.Count;
            }
        }

        public ICollection<TKeyId> IdKeys
        {
            get
            {
                using (globalLocker.ReadLock())
                    return keys.IdKeys;
            }
        }

        public ICollection<TKeyName> NameKeys
        {
            get
            {
                using (globalLocker.ReadLock())
                    return keys.NameKeys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                using (globalLocker.ReadLock())
                    return values.Values;
            }
        }

        public void Clear()
        {
            using (globalLocker.WriteLock())
            {
                keys.Clear();
                values.Clear();
            }
        }

        public bool TryAdd(TKeyId id, TKeyName name, TValue value)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (name == null) throw new ArgumentNullException("name");

            using (globalLocker.ReadLock())
            {
                var mainId = idGenerator.GetId((id, name), out bool isFirst);
                if (!isFirst ) return false;

                var lockNo = GetLockNumber(mainId);
                using (locks[lockNo].WriteLock())
                {
                    var isAdded = keys.TryAdd(id, name);
                    if (!isAdded) return false;
                    values.Add(mainId, value);
                    return true;
                }
            }
        }

        public bool TryGetById(TKeyId id, out Dictionary<TKeyName, TValue> result)
        {
            using (globalLocker.ReadLock())
            {

                throw new NotImplementedException();
            }
        }

        public bool TryGetByName(TKeyName name, out Dictionary<TKeyId, TValue> result)
        {
            using (globalLocker.ReadLock())
            {

                throw new NotImplementedException();
            }
        }

        public bool TryRemove(TKeyId id, TKeyName name)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (name == null) throw new ArgumentNullException("name");

            using (globalLocker.ReadLock())
            {
                var key = idGenerator.GetId((id, name), out bool isFirst);
                if (isFirst) return false;

                var lockNo = GetLockNumber(key);
                using (locks[lockNo].WriteLock())
                {
                    values.Remove(key);
                    keys.TryRemove(id, name);
                }
                return true;
            }
        }

        private int GetLockNumber(long id)
        {
            var count = values.Count == 0 ? 1 : values.Count;
            var localNumber = (id & long.MaxValue) % count;
            var lockNumber = localNumber % locks.Length;
            return (int)lockNumber;
        }

        #region constructors
        public ConcurrentDoubleKeyDictionary()
        {
            keys = new Keys<TKeyId, TKeyName>();
            values = new Dictionary<long, TValue>();

            for (var i = 0; i < locks.Length; i++)
                locks[i] = new RWLock();
        }
        #endregion
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
