using System;
using System.Collections.Generic;

namespace MyCollections
{
    public class DoubleKeyDictionary<TKeyId, TKeyName, TValue> : IDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        private Keys<TKeyId, TKeyName> keys;
        private Dictionary<long, TValue> values;
        IDGenerator idGenerator = new IDGenerator(); 

        public int Count => values.Count;

        public ICollection<TKeyId> IdKeys => keys.IdKeys;

        public ICollection<TKeyName> NameKeys => keys.NameKeys;

        public ICollection<TValue> Values => values.Values;

        public TValue this[TKeyId id, TKeyName name]
        {
            get
            {
                if (id == null || name == null ) throw new ArgumentNullException();
                var key = idGenerator.GetId((id, name), out bool isFirst);
                if(!isFirst) return values[key];
                throw new KeyNotFoundException();
            }
        }

        public void Add(TKeyId id, TKeyName name, TValue value)
        {
            if (id == null || name == null || value == null) throw new ArgumentNullException();
            var mainId = idGenerator.GetId((id, name), out bool isFirst);
            if (!isFirst || !keys.TryAdd(id, name)) throw new ArgumentOutOfRangeException();
            values.Add(mainId, value);
        }

        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }

        public void Remove(TKeyId id, TKeyName name)
        {
            if (id == null || name == null ) throw new ArgumentNullException();
            var key = idGenerator.GetId((id, name), out bool isFirst);
            if(!isFirst)
            {
                values.Remove(key);
                keys.TryRemove(id, name);
            }
        }

        public Dictionary<TKeyName, TValue> GetById(TKeyId id)
        {
            return GetBy<TKeyName, TKeyId>("id", id);
        }

        public Dictionary<TKeyId, TValue> GetByName(TKeyName name)
        {
            return GetBy<TKeyId, TKeyName>("name", name);
        }

        private Dictionary<T1, TValue> GetBy<T1,T2>(string type, T2 key)
        {
            if (key == null) throw new ArgumentNullException();
            var res = new Dictionary<T1, TValue>();

            if (keys.TryGetValue(type, key, out List<T1> id))
                foreach (var x in id)
                {
                    var mainKey = type == "id" ? (object)(key, x) : (object)(x, key);
                    var currentId = idGenerator.GetId(mainKey, out bool isFirst);
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
            
            var key = idGenerator.GetId((id, name), out bool isFirst);
            values.Add(key, value);
        }
        #endregion
    }
}
