using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCollections
{
    internal class Keys<TKeyId, TKeyName>
    {
        private Dictionary<long, List<TKeyName>> _idCollection = new Dictionary<long, List<TKeyName>>();
        private Dictionary<long, List<TKeyId>> _namesCollection = new Dictionary<long, List<TKeyId>>();
        private IDGenerator<TKeyId> _idGenerator = new IDGenerator<TKeyId>();
        private IDGenerator<TKeyName> _nameGenerator = new IDGenerator<TKeyName>();

        public ICollection<TKeyId> IdKeys
        {
            get
            {
                var result = new List<TKeyId>();
                foreach(var key in _namesCollection.Keys)
                {
                    result.AddRange(_namesCollection[key]);
                }
                return result.Distinct().ToList();
            }
        }

        public ICollection<TKeyName> NameKeys
        {
            get
            {
                var result = new List<TKeyName>();
                foreach (var key in _idCollection.Keys)
                {
                    result.AddRange(_idCollection[key]);
                }
                return result.Distinct().ToList();
            }
        }

        public Keys() { }

        public Keys(TKeyId id, TKeyName name)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            var keyId = GetKeyId("id", id, out bool isFirstId);
            var keyName = GetKeyId("name", name, out bool isFirstName);

            _idCollection.Add(keyId, new List<TKeyName>() { name });
            _namesCollection.Add(keyName, new List<TKeyId>() { id });
        }

        public bool TryGetValue<T1, T2>(string type, T1 key, out List<T2> value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            var mainKey = GetKeyId(type, key, out bool isFirst);
            if (isFirst)
            {
                value = new List<T2>();
                return false;
            }
            else
            {
                value = GetDictionary<T2>(type, mainKey);
                return true;
            }
        }

        public void Add(TKeyId id, TKeyName name)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Add("id", id, name);
            Add("name", name, id);
        }

        public void Clear()
        {
            _idCollection.Clear();
            _namesCollection.Clear();
        }

        public void Remove(TKeyId id, TKeyName name)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Remove("id", id, name);
            Remove("name", name, id);
        }

        private void Add<T1, T2>(string type, T1 key, T2 value)
        {
            var mainKey = GetKeyId(type, key, out bool isFirst);
            
            var dictionary = GetCollection<T2>(type);
            if (isFirst)
            {
                dictionary.Add(mainKey, new List<T2>() { value });
            }
            else
            {
                var valueCollection = dictionary[mainKey];
                valueCollection.Add(value);
                dictionary[mainKey] = valueCollection;
            }
        }

        private void Remove<T1, T2>(string type, T1 key, T2 value)
        {
            var mainKey = GetKeyId(type, key, out bool isFirstId);
            if (isFirstId)
            {
                throw new KeyNotFoundException("key");
            }

            var dictionary = GetCollection<T2>(type);
            var valueCollection = dictionary[mainKey];

            if (!valueCollection.Contains(value))
            {
                throw new KeyNotFoundException("key");
            }

            valueCollection.Remove(value);
            if (valueCollection.Count == 0)
            {
                RemoveFromGenerator(type, key);
            }
            else
            {
                dictionary[mainKey] = valueCollection;
            }
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

        private List<T> GetDictionary<T>(string type, long key)
        {
            switch (type)
            {
                case "id":
                    return _idCollection[key] as List<T>;
                case "name":
                    return _namesCollection[key] as List<T>;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Dictionary<long, List<T>> GetCollection<T>(string type)
        {
            switch (type)
            {
                case "id":
                    return _idCollection as Dictionary<long, List<T>>;
                case "name":
                    return _namesCollection as Dictionary<long, List<T>>;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IDGenerator<T> GetIDGenerator<T>(string type)
        {
            switch (type)
            {
                case "id":
                    return _idGenerator as IDGenerator<T>;
                case "name":
                    return _nameGenerator as IDGenerator<T>;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
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
    }
}
