using System;
using System.Collections.Generic;

namespace MyCollections
{
    interface IDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        int Count { get; }
        Dictionary<TKeyName,TValue> GetById(TKeyId id);
        Dictionary<TKeyId, TValue> GetByName(TKeyName name);
        bool TryAdd(TKeyId id, TKeyName name, TValue value);
        void Remove(TKeyId id, TKeyName name);
        void Clear();
    }
}
