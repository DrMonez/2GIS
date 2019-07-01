using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MyCollections
{
    public class DoubleKeyDictionary<TKeyId, TKeyName, TValue> : IDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        private Dictionary<int, List<TKeyName>> idCollection;
        private Dictionary<int, List<TKeyId>> namesCollection;
        private Dictionary<int, TValue> valuesCollection;

        public int Count => valuesCollection.Count;

        public TValue this[TKeyId id, TKeyName name] => valuesCollection[(id, name).GetHashCode()];

        public bool TryAdd(TKeyId id, TKeyName name, TValue value)
        {
            return TryAdd(new Tuple<TKeyId, TKeyName, TValue>(id, name, value));
        }

        public bool TryAdd(Tuple<TKeyId, TKeyName, TValue> elem)
        {
            if (!valuesCollection.ContainsKey((elem.Item1, elem.Item2).GetHashCode()))
            {
                valuesCollection.Add((elem.Item1, elem.Item2).GetHashCode(), elem.Item3);

                if (idCollection.ContainsKey(elem.Item1.GetHashCode()))
                {
                    var tmp = idCollection[elem.Item1.GetHashCode()];
                    tmp.Add(elem.Item2);
                    idCollection[elem.Item1.GetHashCode()] = tmp;
                }
                else idCollection.Add(elem.Item1.GetHashCode(), new List<TKeyName>() { elem.Item2 });

                if (namesCollection.ContainsKey(elem.Item2.GetHashCode()))
                {
                    var tmp = namesCollection[elem.Item2.GetHashCode()];
                    tmp.Add(elem.Item1);
                    namesCollection[elem.Item2.GetHashCode()] = tmp;
                }
                else namesCollection.Add(elem.Item2.GetHashCode(), new List<TKeyId>() { elem.Item1 });
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
            if (valuesCollection.ContainsKey((id, name).GetHashCode()))
            {
                valuesCollection.Remove((id, name).GetHashCode());

                var namesId = idCollection[id.GetHashCode()];
                namesId.Remove(name);
                if (namesId.Count != 0)
                    idCollection[id.GetHashCode()] = namesId;
                else idCollection.Remove(id.GetHashCode());

                var idNames = namesCollection[name.GetHashCode()];
                idNames.Remove(id);
                if (idNames.Count != 0)
                    namesCollection[name.GetHashCode()] = idNames;
                else namesCollection.Remove(name.GetHashCode());
            }
        }

        public Tuple<TKeyName, TValue>[] GetById(TKeyId id)
        {
            List<TKeyName> name;
            if (idCollection.TryGetValue(id.GetHashCode(), out name))
            {
                var res = new List<Tuple<TKeyName, TValue>>();
                foreach (var x in name)
                {
                    res.Add(new Tuple<TKeyName, TValue>(x, valuesCollection[(id, x).GetHashCode()]));
                }
                return res.ToArray();
            }
            else return null;
        }

        public Tuple<TKeyId, TValue>[] GetByName(TKeyName name)
        {
            List<TKeyId> id;
            if (namesCollection.TryGetValue(name.GetHashCode(), out id))
            {
                var res = new List<Tuple<TKeyId, TValue>>();
                foreach (var x in id)
                {
                    res.Add(new Tuple<TKeyId, TValue>(x, valuesCollection[(x, name).GetHashCode()]));
                }
                return res.ToArray();
            }
            else return null;
        }

        #region constructors
        public DoubleKeyDictionary()
        {
            valuesCollection = new Dictionary<int, TValue>();
            idCollection = new Dictionary<int, List<TKeyName>>();
            namesCollection = new Dictionary<int, List<TKeyId>>();
        }

        public DoubleKeyDictionary(Tuple<TKeyId, TKeyName, TValue> elem)
        {
            valuesCollection = new Dictionary<int, TValue>();
            idCollection = new Dictionary<int, List<TKeyName>>();
            namesCollection = new Dictionary<int, List<TKeyId>>();
            
            idCollection.Add(elem.Item1.GetHashCode(), new List<TKeyName>() { elem.Item2 });
            namesCollection.Add(elem.Item2.GetHashCode(), new List<TKeyId>() { elem.Item1 });
            valuesCollection.Add((elem.Item1, elem.Item2).GetHashCode(), elem.Item3);
        }

        public DoubleKeyDictionary(TKeyId id, TKeyName name, TValue value)
        {
            valuesCollection = new Dictionary<int, TValue>();
            idCollection = new Dictionary<int, List<TKeyName>>();
            namesCollection = new Dictionary<int, List<TKeyId>>();
            
            idCollection.Add(id.GetHashCode(), new List<TKeyName>() { name });
            namesCollection.Add(name.GetHashCode(), new List<TKeyId>() { id });
            valuesCollection.Add((id, name).GetHashCode(), value);
        }
        #endregion
    }
}
