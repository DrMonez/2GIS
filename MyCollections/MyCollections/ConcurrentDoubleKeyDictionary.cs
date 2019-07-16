using System;
using System.Collections.Generic;
using System.Text;

namespace MyCollections
{
    class ConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue> : IConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        private Keys<TKeyId, TKeyName> keys;
        private Dictionary<long, TValue> values;
        IDGenerator idGenerator = new IDGenerator();

        public int Count => values.Count;

        public ICollection<TKeyId> IdKeys => keys.IdKeys;

        public ICollection<TKeyName> NameKeys => keys.NameKeys;

        public ICollection<TValue> Values => values.Values;

        public TValue AddOrUpdate(TKeyId id, TKeyName name, TValue value, Func<TKeyId, TKeyName, TValue, TValue> updateValueFactory)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public Dictionary<TKeyName, TValue> GetByIdOrAdd(TKeyId id, Func<TKeyId, TKeyName, TValue, TValue> valueFactory)
        {
            throw new NotImplementedException();
        }

        public Dictionary<TKeyId, TValue> GetByNameOrAdd(TKeyName name, Func<TKeyId, TKeyName, TValue, TValue> valueFactory)
        {
            throw new NotImplementedException();
        }

        public bool TryAdd(TKeyId id, TKeyName name, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetById(TKeyId id, out Dictionary<TKeyName, TValue> result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetByName(TKeyName name, out Dictionary<TKeyId, TValue> result)
        {
            throw new NotImplementedException();
        }

        public bool TryRemove(TKeyId id, TKeyName name)
        {
            throw new NotImplementedException();
        }
    }
}
