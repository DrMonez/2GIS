using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyCollections
{
    public class ConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue> : IConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        //private readonly int _threadsCount = 31;
        private Keys<TKeyId, TKeyName> _keys;
        private Dictionary<long, TValue> _values;
        private IDGenerator _idGenerator = new IDGenerator();
        private RWLock[] _locks = new RWLock[31];
        private RWLock _globalLocker = new RWLock();

        public int Count
        {
            get
            {
                using (_globalLocker.ReadLock())
                    return _values.Count;
            }
        }

        public ICollection<TKeyId> IdKeys
        {
            get
            {
                using (_globalLocker.ReadLock())
                    return _keys.IdKeys;
            }
        }

        public ICollection<TKeyName> NameKeys
        {
            get
            {
                using (_globalLocker.ReadLock())
                    return _keys.NameKeys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                using (_globalLocker.ReadLock())
                    return _values.Values;
            }
        }

        public void Clear()
        {
            using (_globalLocker.WriteLock())
            {
                _keys.Clear();
                _values.Clear();
            }
        }

        public ConcurrentDoubleKeyDictionary()
        {
            _keys = new Keys<TKeyId, TKeyName>();
            _values = new Dictionary<long, TValue>();

            for (var i = 0; i < _locks.Length; i++)
            {
                _locks[i] = new RWLock();
            }
        }

        public bool TryAdd(TKeyId id, TKeyName name, TValue value)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            using (_globalLocker.ReadLock())
            {
                var mainId = _idGenerator.GetId((id, name), out bool isFirst);
                if (!isFirst)
                {
                    return false;
                }

                var lockNo = GetLockNumber(mainId);
                using (_locks[lockNo].WriteLock())
                {
                    var isAdded = _keys.TryAdd(id, name);
                    if (!isAdded)
                    {
                        return false;
                    }
                    _values.Add(mainId, value);
                    return true;
                }
            }
        }

        public bool TryGetById(TKeyId id, out Dictionary<TKeyName, TValue> result)
        {
            using (_globalLocker.ReadLock())
            {

                throw new NotImplementedException();
            }
        }

        public bool TryGetByName(TKeyName name, out Dictionary<TKeyId, TValue> result)
        {
            using (_globalLocker.ReadLock())
            {

                throw new NotImplementedException();
            }
        }

        public bool TryRemove(TKeyId id, TKeyName name)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            using (_globalLocker.ReadLock())
            {
                var key = _idGenerator.GetId((id, name), out bool isFirst);
                if (isFirst) return false;

                var lockNo = GetLockNumber(key);
                using (_locks[lockNo].WriteLock())
                {
                    _values.Remove(key);
                    _keys.TryRemove(id, name);
                }
                return true;
            }
        }

        private int GetLockNumber(long id)
        {
            var count = _values.Count == 0 ? 1 : _values.Count;
            var localNumber = (id & long.MaxValue) % count;
            var lockNumber = localNumber % _locks.Length;
            return (int)lockNumber;
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

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public ReadLockToken ReadLock() => new ReadLockToken(_lock);
        public WriteLockToken WriteLock() => new WriteLockToken(_lock);

        public void Dispose() => _lock.Dispose();
    }

}
