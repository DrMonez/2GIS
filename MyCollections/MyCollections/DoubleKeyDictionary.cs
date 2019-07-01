using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MyCollections
{
    public class DoubleKeyDictionary<TKeyId, TKeyName, TValue> : IDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        private Dictionary<TKeyId, List<TKeyName>> idCollection;
        private Dictionary<TKeyName, List<TKeyId>> namesCollection;
        private Dictionary<(TKeyId, TKeyName), TValue> valuesCollection;

        public int Count => valuesCollection.Count;

        public TValue this[TKeyId id, TKeyName name] => valuesCollection[(id, name)];

        public bool TryAdd(TKeyId id, TKeyName name, TValue value)
        {
            return TryAdd(new Tuple<TKeyId, TKeyName, TValue>(id, name, value));
        }

        public bool TryAdd(Tuple<TKeyId, TKeyName, TValue> elem)
        {
            if (!valuesCollection.ContainsKey((elem.Item1, elem.Item2)))
            {
                valuesCollection.Add((elem.Item1, elem.Item2), elem.Item3);

                if (idCollection.ContainsKey(elem.Item1))
                {
                    var tmp = idCollection[elem.Item1];
                    tmp.Add(elem.Item2);
                    idCollection[elem.Item1] = tmp;
                }
                else idCollection.Add(elem.Item1, new List<TKeyName>() { elem.Item2 });

                if (namesCollection.ContainsKey(elem.Item2))
                {
                    var tmp = namesCollection[elem.Item2];
                    tmp.Add(elem.Item1);
                    namesCollection[elem.Item2] = tmp;
                }
                else namesCollection.Add(elem.Item2, new List<TKeyId>() { elem.Item1 });
            }
            else return false;
            return true;
        }

        public void Clear()
        {
            idCollection.Clear();
            namesCollection.Clear();
            valuesCollection.Clear();
        }

        public void Remove(TKeyId id, TKeyName name)
        {
            if (valuesCollection.ContainsKey((id, name)))
            {
                valuesCollection.Remove((id, name));

                var namesId = idCollection[id];
                namesId.Remove(name);
                if (namesId.Count != 0)
                    idCollection[id] = namesId;
                else idCollection.Remove(id);

                var idNames = namesCollection[name];
                idNames.Remove(id);
                if (idNames.Count != 0)
                    namesCollection[name] = idNames;
                else namesCollection.Remove(name);
            }
        }

        public Tuple<TKeyName, TValue>[] GetById(TKeyId id)
        {
            List<TKeyName> name;
            if (idCollection.TryGetValue(id, out name))
            {
                var res = new List<Tuple<TKeyName, TValue>>();
                foreach (var x in name)
                {
                    res.Add(new Tuple<TKeyName, TValue>(x, valuesCollection[(id, x)]));
                }
                return res.ToArray();
            }
            else return null;
        }

        public Tuple<TKeyId, TValue>[] GetByName(TKeyName name)
        {
            List<TKeyId> id;
            if (namesCollection.TryGetValue(name, out id))
            {
                var res = new List<Tuple<TKeyId, TValue>>();
                foreach (var x in id)
                {
                    res.Add(new Tuple<TKeyId, TValue>(x, valuesCollection[(x, name)]));
                }
                return res.ToArray();
            }
            else return null;
        }

        #region constructors
        public DoubleKeyDictionary()
        {
            valuesCollection = new Dictionary<(TKeyId, TKeyName), TValue>();
            idCollection = new Dictionary<TKeyId, List<TKeyName>>();
            namesCollection = new Dictionary<TKeyName, List<TKeyId>>();
        }

        public DoubleKeyDictionary(Tuple<TKeyId, TKeyName, TValue> elem)
        {
            valuesCollection = new Dictionary<(TKeyId, TKeyName), TValue>();
            idCollection = new Dictionary<TKeyId, List<TKeyName>>();
            namesCollection = new Dictionary<TKeyName, List<TKeyId>>();
            
            idCollection.Add(elem.Item1, new List<TKeyName>() { elem.Item2 });
            namesCollection.Add(elem.Item2, new List<TKeyId>() { elem.Item1 });
            valuesCollection.Add((elem.Item1, elem.Item2), elem.Item3);
        }

        public DoubleKeyDictionary(TKeyId id, TKeyName name, TValue value)
        {
            valuesCollection = new Dictionary<(TKeyId, TKeyName), TValue>();
            idCollection = new Dictionary<TKeyId, List<TKeyName>>();
            namesCollection = new Dictionary<TKeyName, List<TKeyId>>();
            
            idCollection.Add(id, new List<TKeyName>() { name });
            namesCollection.Add(name, new List<TKeyId>() { id });
            valuesCollection.Add((id, name), value);
        }
        #endregion
    }
}
