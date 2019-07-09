using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace MyCollections
{
    public class DoubleKeyDictionary<TKeyId, TKeyName, TValue> : IDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        private Keys<TKeyId, TKeyName> keys;
        private Dictionary<long, TValue> values;
        ObjectIDGenerator idGenerator = new ObjectIDGenerator(); 

        public int Count => values.Count;

        public TValue this[TKeyId id, TKeyName name]
        {
            get
            {
                bool isFirst;
                var key = idGenerator.GetId((id, name), out isFirst);
                if(!isFirst) return values[key];
                throw new KeyNotFoundException();
            }
        }

        public bool TryAdd(TKeyId id, TKeyName name, TValue value)
        {
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
            bool isFirst;
            var key = idGenerator.GetId((id, name), out isFirst);
            if(!isFirst)
            {
                values.Remove(key);
                keys.Remove(id, name);
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
            values = new Dictionary<long, TValue>();
            keys = new Keys<TKeyId, TKeyName>(id, name);
            values.Add((id, name).GetHashCode(), value);
        }
        #endregion
    }
    

    public sealed class Keys<TKeyId, TKeyName> 
    {
        private Dictionary<long, List<TKeyName>> idCollection = new Dictionary<long, List<TKeyName>>();
        private Dictionary<long, List<TKeyId>> namesCollection = new Dictionary<long, List<TKeyId>>();
        ObjectIDGenerator idGenerator = new ObjectIDGenerator();

        public bool TryGetValue<TKey, TValue>(TKey key, out List<TValue> value)
        {
            throw new NotImplementedException();
        }

        public bool TryAdd(TKeyId id, TKeyName name)
        {
            bool isFirstId;
            var keyId = idGenerator.GetId(id, out isFirstId);
            bool isFirstName;
            var keyName = idGenerator.GetId(name, out isFirstName);

            if (!isFirstId || !isFirstName) return false;

            var tmpId = idCollection[keyId];
            tmpId.Add(name);
            idCollection[keyId] = tmpId;

            var tmpName = namesCollection[keyName];
            tmpName.Add(id);
            namesCollection[keyName] = tmpName;
            return true;
        }

        public void Clear()
        {
            idCollection.Clear();
            namesCollection.Clear();
        }

        public void Remove(TKeyId id, TKeyName name)
        {

        }

        public Keys(TKeyId id, TKeyName name)
        {
            idCollection = new Dictionary<long, List<TKeyName>>();
            namesCollection = new Dictionary<long, List<TKeyId>>();

            bool isFirstId;
            var keyId = idGenerator.GetId(id, out isFirstId);
            bool isFirstName;
            var keyName = idGenerator.GetId(name, out isFirstName);

            idCollection.Add(keyId, new List<TKeyName>() { name });
            namesCollection.Add(keyName, new List<TKeyId>() { id });
        }

        public Keys() { }
    }
}
