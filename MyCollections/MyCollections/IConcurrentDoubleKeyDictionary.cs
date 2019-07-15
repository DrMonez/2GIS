using System;
using System.Collections.Generic;
using System.Text;

namespace MyCollections
{
    interface IConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        int Count { get; }
        bool TryGetById(TKeyId id, out Dictionary<TKeyName, TValue> result);
        bool TryGetByName(TKeyName name, out Dictionary<TKeyId, TValue> result);
        Dictionary<TKeyName, TValue> GetByIdOrAdd(TKeyId id, Func<TKeyId, TKeyName, TValue, TValue> valueFactory);
        Dictionary<TKeyId, TValue> GetByNameOrAdd(TKeyName name, Func<TKeyId, TKeyName, TValue, TValue> valueFactory);
        bool TryAdd(TKeyId id, TKeyName name, TValue value);
        bool TryRemove(TKeyId id, TKeyName name);
        TValue AddOrUpdate(TKeyId id, TKeyName name, TValue value, Func<TKeyId, TKeyName, TValue, TValue> updateValueFactory);
        void Clear();
        ICollection<TKeyId> IdKeys { get; }
        ICollection<TKeyName> NameKeys { get; }
        ICollection<TValue> Values { get; }
    }
}
