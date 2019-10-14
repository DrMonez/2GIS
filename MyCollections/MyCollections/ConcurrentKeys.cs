using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCollections
{
    internal class ConcurrentKeys<TKeyId, TKeyName>
    {
        private int _idsCount = 0;
        private int _namesCount = 0;
        private Dictionary<long, List<TKeyName>>[] _idsCollection = new Dictionary<long, List<TKeyName>>[Constants.MaxThreadsCount];
        private Dictionary<long, List<TKeyId>>[] _namesCollection = new Dictionary<long, List<TKeyId>>[Constants.MaxThreadsCount];
        private ConcurrentIDGenerator<TKeyId> _idGenerator = new ConcurrentIDGenerator<TKeyId>();
        private ConcurrentIDGenerator<TKeyName> _nameGenerator = new ConcurrentIDGenerator<TKeyName>();
        private RWLock _globalLocker = new RWLock();


        public ICollection<TKeyId> IdKeys
        {
            get
            {
                using (_globalLocker.ReadLock())
                {
                    var result = new List<TKeyId>();
                    foreach (var names in _namesCollection)
                    {
                        lock (names)
                        {
                            foreach (var name in names.Keys)
                            {
                                result.AddRange(names[name]);
                            }
                        }
                    }
                    return result.Distinct().ToList();
                }
            }
        }

        public ICollection<TKeyName> NameKeys
        {
            get
            {
                using (_globalLocker.ReadLock())
                {
                    var result = new List<TKeyName>();
                    foreach (var ids in _idsCollection)
                    {
                        lock (ids)
                        {
                            foreach (var id in ids.Keys)
                            {
                                result.AddRange(ids[id]);
                            }
                        }
                    }
                    return result.Distinct().ToList();
                }
            }
        }

        public ConcurrentKeys()
        {
            using (_globalLocker.WriteLock())
            {
                for (var i = 0; i < Constants.MaxThreadsCount; i++)
                {
                    _idsCollection[i] = new Dictionary<long, List<TKeyName>>();
                    _namesCollection[i] = new Dictionary<long, List<TKeyId>>();
                }
            }
        }

        public bool TryGetValue<T1, T2>(string type, T1 key, out List<T2> value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            using (_globalLocker.ReadLock())
            {
                var mainKey = GetKeyId(type, key, out bool isFirst);
                if (isFirst)
                {
                    value = new List<T2>();
                    return false;
                }

                var lockNo = GetLockNumber(mainKey);
                var dictionary = GetDictionary<T2>(type, lockNo);
                lock (dictionary)
                {
                    value = dictionary[mainKey];
                    return true;
                }
            }
        }

        public bool TryAdd(TKeyId id, TKeyName name)
        {
            if (id == null || name == null)
            {
                return false;
            }
            using (_globalLocker.ReadLock())
            {
                Add("id", id, name);
                Add("name", name, id);
            }
            using (_globalLocker.WriteLock())
            {
                _idsCount++;
                _namesCount++;
                return true;
            }
        }

        public void Clear()
        {
            using (_globalLocker.WriteLock())
            {
                for (var i = 0; i < Constants.MaxThreadsCount; i++)
                {
                    _idsCount = 0;
                    _namesCount = 0;
                    _idsCollection[i].Clear();
                    _namesCollection[i].Clear();
                }
            }
        }

        public bool TryRemove(TKeyId id, TKeyName name)
        {
            if (id == null || name == null)
            {
                return false;
            }
            bool removeId, removeName;
            using (_globalLocker.ReadLock())
            {
                removeId = TryRemove("id", id, name);
                removeName = TryRemove("name", name, id);
            }
            using (_globalLocker.WriteLock())
            {
                if (removeId)
                {
                    _idsCount--;
                }
                if (removeName)
                {
                    _namesCount--;
                }
                return removeId && removeName;
            }
        }

        private void Add<T1, T2>(string type, T1 key, T2 value)
        {
            var mainKey = GetKeyId(type, key, out bool isFirst);

            var lockNo = GetLockNumber(mainKey);
            var dictionary = GetCollection<T2>(type);
            lock (dictionary[lockNo])
            {
                if (isFirst)
                {
                    dictionary[lockNo].Add(mainKey, new List<T2>() { value });
                }
                else
                {
                    var valueCollection = dictionary[lockNo][mainKey];
                    valueCollection.Add(value);
                    dictionary[lockNo][mainKey] = valueCollection;
                }
            }
        }

        private bool TryRemove<T1, T2>(string type, T1 key, T2 value)
        {
            var mainKey = GetKeyId(type, key, out bool isFirstId);
            if (isFirstId)
            {
                return false;
            }

            var lockNo = GetLockNumber(mainKey);
            var dictionary = GetCollection<T2>(type);
            lock (dictionary[lockNo])
            {
                var valueCollection = dictionary[lockNo][mainKey];

                if (!valueCollection.Contains(value))
                {
                    return false;
                }

                valueCollection.Remove(value);
                if (valueCollection.Count == 0)
                {
                    RemoveFromGenerator(type, key);
                }
                else
                {
                    dictionary[lockNo][mainKey] = valueCollection;
                }
               
            }
            return true;
        }

        private void RemoveFromGenerator<T>(string type, T key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            var generator = GetIDGenerator<T>(type);
            generator.Remove(key);
        }

        private long GetKeyId<T>(string type, T key, out bool isFirst)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            var generator = GetIDGenerator<T>(type);
            var resultId = generator.GetId(key, out isFirst);
            return resultId;
        }

        private Dictionary<long, List<T>> GetDictionary<T>(string type, int index)
        {
            var result = new Dictionary<long, List<T>>();
            switch (type)
            {
                case "id":
                    result = _idsCollection[index] as Dictionary<long, List<T>>;
                    break;
                case "name":
                    result = _namesCollection[index] as Dictionary<long, List<T>>;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        private Dictionary<long, List<T>>[] GetCollection<T>(string type)
        {
            switch (type)
            {
                case "id":
                    return _idsCollection as Dictionary<long, List<T>>[];
                case "name":
                    return _namesCollection as Dictionary<long, List<T>>[];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ConcurrentIDGenerator<T> GetIDGenerator<T>(string type)
        {
            switch (type)
            {
                case "id":
                    return _idGenerator as ConcurrentIDGenerator<T>;
                case "name":
                    return _nameGenerator as ConcurrentIDGenerator<T>;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        private int GetLockNumber(long id)
        {
            var intNumber = (int)(id % int.MaxValue);
            var lockNumber = intNumber % Constants.MaxThreadsCount;
            return lockNumber;
        }
    }
}
