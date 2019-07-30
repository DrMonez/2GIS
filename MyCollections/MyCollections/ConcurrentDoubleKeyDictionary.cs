using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyCollections
{
    public class ConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue> : IConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        private int _count = 0;
        private ConcurrentKeys<TKeyId, TKeyName> _keys;
        private Dictionary<long, TValue>[] _values = new Dictionary<long, TValue> [Constants.MaxThreadsCount];
        private ConcurrentIDGenerator _idGenerator = new ConcurrentIDGenerator();
        private RWLock _globalLocker = new RWLock();

        public int Count
        {
            get
            {
                using (_globalLocker.ReadLock())
                {
                    return _count;
                }
            }
        }

        public ICollection<TKeyId> IdKeys
        {
            get
            {
                using (_globalLocker.ReadLock())
                {
                    return _keys.IdKeys;
                }
            }
        }

        public ICollection<TKeyName> NameKeys
        {
            get
            {
                using (_globalLocker.ReadLock())
                {
                    return _keys.NameKeys;
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                using (_globalLocker.ReadLock())
                {
                    var result = new TValue[_count];
                    var index = 0;
                    foreach (var values in _values)
                    {
                        lock (values)
                        {
                            var valuesOfDictionary = new Dictionary<long, TValue>.ValueCollection(values);
                            valuesOfDictionary.CopyTo(result, index);
                            index += values.Count;
                        }
                    }
                    return result;
                }
            }
        }

        public void Clear()
        {
            using (_globalLocker.WriteLock())
            {
                _keys.Clear();
                foreach(var values in _values)
                {
                    values.Clear();
                }
            }
        }

        public ConcurrentDoubleKeyDictionary()
        {
            _keys = new ConcurrentKeys<TKeyId, TKeyName>();
            for (var i = 0; i < Constants.MaxThreadsCount; i++)
            {
                _values[i] = new Dictionary<long, TValue>();
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
                lock(_values[lockNo])
                {
                    var isAdded = _keys.TryAdd(id, name);
                    if (!isAdded)
                    {
                        return false;
                    }
                    lock (_values)
                    {
                        _values[lockNo].Add(mainId, value);
                    }
                    _count++;
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
                lock(_values[lockNo])
                {
                    _values[lockNo].Remove(key);
                    _keys.TryRemove(id, name);
                }
                return true;
            }
        }

        private int GetLockNumber(long id)
        {
            var lockNumber = (int)id % Constants.MaxThreadsCount;
            return lockNumber;
        }
    }
}
