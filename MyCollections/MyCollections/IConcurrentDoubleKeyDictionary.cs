using System;
using System.Collections.Generic;
using System.Text;

namespace MyCollections
{
    public interface IConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        int Count { get; }
        bool TryGetById(TKeyId id, out Dictionary<TKeyName, TValue> result);
        bool TryGetByName(TKeyName name, out Dictionary<TKeyId, TValue> result);
        bool TryAdd(TKeyId id, TKeyName name, TValue value);
        bool TryRemove(TKeyId id, TKeyName name);
        void Clear();
        ICollection<TKeyId> IdKeys { get; }
        ICollection<TKeyName> NameKeys { get; }
        ICollection<TValue> Values { get; }
    }
}
