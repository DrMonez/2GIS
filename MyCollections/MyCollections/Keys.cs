using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCollections
{
    public sealed class Keys<TKeyId, TKeyName>
    {
        private Dictionary<long, List<TKeyName>> idCollection = new Dictionary<long, List<TKeyName>>();
        private Dictionary<long, List<TKeyId>> namesCollection = new Dictionary<long, List<TKeyId>>();
        IDGenerator idGenerator = new IDGenerator();
        IDGenerator nameGenerator = new IDGenerator();

        public ICollection<TKeyId> IdKeys
        {
            get
            {
                var res = new List<TKeyId>();
                foreach(var key in namesCollection.Keys)
                {
                    res.AddRange(namesCollection[key]);
                }
                return res.Distinct().ToList();
            }
        }

        public ICollection<TKeyName> NameKeys
        {
            get
            {
                var res = new List<TKeyName>();
                foreach (var key in idCollection.Keys)
                {
                    res.AddRange(idCollection[key]);
                }
                return res.Distinct().ToList();
            }
        }

        public bool TryGetValue<T1, T2>(string type, T1 key, out List<T2> value)
        {
            if (key == null) throw new ArgumentNullException();
            bool isFirst;
            var mainKey = GetKeyId(type, key, out isFirst);
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
            if (id == null || name == null) throw new ArgumentNullException();
            var isAddId = TryAdd("id", id, name);
            var isAddName = TryAdd("name", name, id);
            return true;
        }

        public void Clear()
        {
            idCollection.Clear();
            namesCollection.Clear();
        }

        public void TryRemove(TKeyId id, TKeyName name)
        {
            if (id == null || name == null) throw new ArgumentNullException();
            Remove("id", id, name);
            Remove("name", name, id);
        }

        private bool TryAdd<T1, T2>(string type, T1 key, T2 value)
        {
            if (key == null || value == null) throw new ArgumentNullException();
            bool isFirst;
            var mainKey = GetKeyId(type, key, out isFirst);

            var dictionary = GetCollection<T2>(type);
            if (isFirst) dictionary.Add(mainKey, new List<T2>() { value });
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
            bool isFirstId;
            var mainKey = GetKeyId(type, key, out isFirstId);
            if (isFirstId) throw new KeyNotFoundException();

            var dictionary = GetCollection<T2>(type);
            var tmpId = dictionary[mainKey];
            if (!tmpId.Contains(value)) throw new KeyNotFoundException();
            tmpId.Remove(value);
            dictionary[mainKey] = tmpId;
        }

        private long GetKeyId<T>(string type, T key, out bool isFirst)
        {
            long resultId = 0;
            switch (type)
            {
                case "id":
                    resultId = idGenerator.GetId(key, out isFirst);
                    break;
                case "name":
                    resultId = nameGenerator.GetId(key, out isFirst);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return resultId;
        }

        private List<T> GetDictionary<T>(string type, long key)
        {
            var res = new List<T>();
            switch (type)
            {
                case "id":
                    res = idCollection[key] as List<T>;
                    break;
                case "name":
                    res = namesCollection[key] as List<T>;
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
                    return idCollection as Dictionary<long, List<T>>;
                case "name":
                    return namesCollection as Dictionary<long, List<T>>;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Keys() { }

        public Keys(TKeyId id, TKeyName name)
        {
            if (id == null || name == null) throw new ArgumentNullException();
            idCollection = new Dictionary<long, List<TKeyName>>();
            namesCollection = new Dictionary<long, List<TKeyId>>();

            bool isFirstId;
            var keyId = GetKeyId("id", id, out isFirstId);
            bool isFirstName;
            var keyName = GetKeyId("name", name, out isFirstName);

            idCollection.Add(keyId, new List<TKeyName>() { name });
            namesCollection.Add(keyName, new List<TKeyId>() { id });
        }
    }
}
