﻿using System;
using System.Collections.Generic;

namespace MyCollections
{
    public class DoubleKeyDictionary<TKeyId, TKeyName, TValue> : IDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        private Keys<TKeyId, TKeyName> keys;
        private Dictionary<long, TValue> values;
        IDGenerator idGenerator = new IDGenerator(); 

        public int Count => values.Count;

        public TValue this[TKeyId id, TKeyName name]
        {
            get
            {
                if (id == null || name == null ) throw new ArgumentNullException();
                bool isFirst;
                var key = idGenerator.GetId((id, name), out isFirst);
                if(!isFirst) return values[key];
                throw new KeyNotFoundException();
            }
        }

        public bool TryAdd(TKeyId id, TKeyName name, TValue value)
        {
            if (id == null || name == null || value == null) throw new ArgumentNullException();
            bool isFirst;
            var mainId = idGenerator.GetId((id, name), out isFirst);
            if (!isFirst) return false;
            if (!keys.TryAdd(id, name)) return false;
            values.Add(mainId, value);
            return true;
        }

        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }

        public void Remove(TKeyId id, TKeyName name)
        {
            if (id == null || name == null ) throw new ArgumentNullException();
            bool isFirst;
            var key = idGenerator.GetId((id, name), out isFirst);
            if(!isFirst)
            {
                values.Remove(key);
                keys.TryRemove(id, name);
            }
        }

        public Dictionary<TKeyName, TValue> GetById(TKeyId id)
        {
            return GetBy<TKeyName, TKeyId>(id);
        }

        public Dictionary<TKeyId, TValue> GetByName(TKeyName name)
        {
            return GetBy<TKeyId, TKeyName>(name);
        }

        private Dictionary<T1, TValue> GetBy<T1,T2>(T2 key)
        {
            if (key == null) throw new ArgumentNullException();
            var res = new Dictionary<T1, TValue>();
            List<T1> id;
            if (keys.TryGetValue(key, out id))
                foreach (var x in id)
                {
                    bool isFirst;
                    var mainKey = typeof(T2) == typeof(TKeyId) ? (object)(key, x) : (object)(x, key);
                    var currentId = idGenerator.GetId(mainKey, out isFirst);
                    if(!isFirst) res.Add(x, values[currentId]);
                }
            return res;
        }

        #region constructors
        public DoubleKeyDictionary()
        {
            values = new Dictionary<long, TValue>();
            keys = new Keys<TKeyId, TKeyName>();
        }

        public DoubleKeyDictionary(TKeyId id, TKeyName name, TValue value)
        {
            if (id == null || name == null || value == null) throw new ArgumentNullException();
            values = new Dictionary<long, TValue>();
            keys = new Keys<TKeyId, TKeyName>(id, name);

            bool isFirst;
            var key = idGenerator.GetId((id, name), out isFirst);
            values.Add(key, value);
        }
        #endregion
    }
    

    public sealed class Keys<TKeyId, TKeyName> 
    {
        private Dictionary<long, List<TKeyName>> idCollection = new Dictionary<long, List<TKeyName>>();
        private Dictionary<long, List<TKeyId>> namesCollection = new Dictionary<long, List<TKeyId>>();
        IDGenerator idGenerator = new IDGenerator();
        IDGenerator nameGenerator = new IDGenerator();

        public bool TryGetValue<T1,T2>(T1 key, out List<T2> value)
        {
            if (key == null) throw new ArgumentNullException();
            bool isFirst;
            var mainKey = GetKeyId(key, out isFirst);
            if(isFirst)
            {
                value = new List<T2>();
                return false;
            }
            if (typeof(T1) == typeof(TKeyId))
                value = idCollection[mainKey] as List<T2>;
            else
                value = namesCollection[mainKey] as List<T2>;
            return true;
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
                    throw new ArgumentException();
            }
            return resultId;
        }

        public bool TryAdd(TKeyId id, TKeyName name)
        {
            if (id == null || name == null) throw new ArgumentNullException();
            var isAddId = TryAdd("id", id, name);
            var isAddName = TryAdd("name", name, id);
            return true;
        }

        private bool TryAdd<T1, T2>(string type, T1 key, T2 value)
        {
            if (key == null || value == null) throw new ArgumentNullException();
            bool isFirst;
            var mainKey = GetKeyId(type, key, out isFirst);
            if (isFirst) dictionary.Add(mainKey, new List<T2>() { value });
            else
            {
                var tmp = dictionary[mainKey];
                tmp.Add(value);
                dictionary[mainKey] = tmp;
            }
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

        private void Remove<T1, T2>(string type, T1 key, T2 value)
        {
            bool isFirstId;
            var mainKey = GetKeyId(type, key, out isFirstId);
            if (isFirstId) throw new KeyNotFoundException();

            var tmpId = dictionary[mainKey];
            if (!tmpId.Contains(value)) throw new KeyNotFoundException();
            tmpId.Remove(value);
            dictionary[mainKey] = tmpId;
        }

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

        public Keys() { }
    }
}
