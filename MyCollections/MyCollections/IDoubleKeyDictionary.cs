using System;

namespace MyCollections
{
    interface IDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        int Count { get; }
        Tuple<TKeyName, TValue> GetById(TKeyId id);
        Tuple<TKeyId, TValue> GetByName(TKeyName name);
        bool TryAdd(TKeyId id, TKeyName name, TValue value);
        bool TryAdd(Tuple<TKeyId, TKeyName, TValue> elem);
        void Remove(TKeyId id);
        void Remove(TKeyName name);
        void Clear();
    }
}
