using System;
using System.Collections.Generic;

namespace MyCollections
{
    public interface IDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        int Count { get; }
        Dictionary<TKeyName,TValue> GetById(TKeyId id);
        Dictionary<TKeyId, TValue> GetByName(TKeyName name);
        void Add(TKeyId id, TKeyName name, TValue value);
        void Remove(TKeyId id, TKeyName name);
        void Clear();
        ICollection<TKeyId> IdKeys { get; }
        ICollection<TKeyName> NameKeys { get; }
        ICollection<TValue> Values { get; }
    }
}
