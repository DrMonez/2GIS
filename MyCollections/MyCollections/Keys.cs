using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCollections
{
    public class Keys<TKeyId, TKeyName>
    {
        private Dictionary<long, List<TKeyName>> _idCollection = new Dictionary<long, List<TKeyName>>();
        private Dictionary<long, List<TKeyId>> _namesCollection = new Dictionary<long, List<TKeyId>>();
        private IDGenerator _idGenerator = new IDGenerator();
        private IDGenerator _nameGenerator = new IDGenerator();

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
                var res = new List<TKeyName>();
                foreach (var key in _idCollection.Keys)
                {
                    res.AddRange(_idCollection[key]);
                }
                return res.Distinct().ToList();
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

            _idCollection = new Dictionary<long, List<TKeyName>>();
            _namesCollection = new Dictionary<long, List<TKeyId>>();

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
            value = GetDictionary<T2>(type, mainKey);
            return true;
        }

        public bool TryAdd(TKeyId id, TKeyName name)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            var isAddId = TryAdd("id", id, name);
            var isAddName = TryAdd("name", name, id);
            return true;
        }

        public void Clear()
        {
            _idCollection.Clear();
            _namesCollection.Clear();
        }

        public void TryRemove(TKeyId id, TKeyName name)
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

        private bool TryAdd<T1, T2>(string type, T1 key, T2 value)
        {
            var mainKey = GetKeyId(type, key, out bool isFirst);
            
            var dictionary = GetCollection<T2>(type);
            if (isFirst)
            {
                dictionary.Add(mainKey, new List<T2>() { value });
            }
            else
            {
                var tmp = dictionary[mainKey];
                tmp.Add(value);
                dictionary[mainKey] = tmp;
            }
            return true;
        }

        private void Remove<T1, T2>(string type, T1 key, T2 value)
        {
            var mainKey = GetKeyId(type, key, out bool isFirstId);
            if (isFirstId)
            {
                throw new KeyNotFoundException("key");
            }

            var dictionary = GetCollection<T2>(type);
            var tmpId = dictionary[mainKey];

            if (!tmpId.Contains(value))
            {
                throw new KeyNotFoundException("key");
            }

            tmpId.Remove(value);
            dictionary[mainKey] = tmpId;
        }

        private long GetKeyId<T>(string type, T key, out bool isFirst)
        {
            if(type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            long resultId = 0;
            switch (type)
            {
                case "id":
                    resultId = _idGenerator.GetId(key, out isFirst);
                    break;
                case "name":
                    resultId = _nameGenerator.GetId(key, out isFirst);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return resultId;
        }

        private List<T> GetDictionary<T>(string type, long key)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var res = new List<T>();
            switch (type)
            {
                case "id":
                    res = _idCollection[key] as List<T>;
                    break;
                case "name":
                    res = _namesCollection[key] as List<T>;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return res;
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
    }
}
