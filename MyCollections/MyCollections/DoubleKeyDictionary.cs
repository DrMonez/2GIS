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
        private Dictionary<long, TValue> valuesCollection;
        ObjectIDGenerator idGenerator = new ObjectIDGenerator(); 

        public int Count => valuesCollection.Count;

        public TValue this[TKeyId id, TKeyName name] => valuesCollection[(id, name).GetHashCode()];

        public bool TryAdd(TKeyId id, TKeyName name, TValue value)
        {
            return TryAdd(new Tuple<TKeyId, TKeyName, TValue>(id, name, value));
        }

        public bool TryAdd(Tuple<TKeyId, TKeyName, TValue> elem)
        {
            //if (!valuesCollection.ContainsKey((elem.Item1, elem.Item2).GetHashCode()))
            //{
            //    valuesCollection.Add((elem.Item1, elem.Item2).GetHashCode(), elem.Item3);

            //    if (idCollection.ContainsKey(elem.Item1.GetHashCode()))
            //    {
            //        var tmp = idCollection[elem.Item1.GetHashCode()];
            //        tmp.Add(elem.Item2);
            //        idCollection[elem.Item1.GetHashCode()] = tmp;
            //    }
            //    else idCollection.Add(elem.Item1.GetHashCode(), new List<TKeyName>() { elem.Item2 });

            //    if (namesCollection.ContainsKey(elem.Item2.GetHashCode()))
            //    {
            //        var tmp = namesCollection[elem.Item2.GetHashCode()];
            //        tmp.Add(elem.Item1);
            //        namesCollection[elem.Item2.GetHashCode()] = tmp;
            //    }
            //    else namesCollection.Add(elem.Item2.GetHashCode(), new List<TKeyId>() { elem.Item1 });
            //}
            //else return false;
            return true;
        }

        public void Clear()
        {
            //idCollection.Clear();
            //namesCollection.Clear();
            //valuesCollection.Clear();
        }

        public void Remove(TKeyId id, TKeyName name)
        {
            //if (valuesCollection.ContainsKey((id, name).GetHashCode()))
            //{
            //    valuesCollection.Remove((id, name).GetHashCode());

            //    var namesId = idCollection[id.GetHashCode()];
            //    namesId.Remove(name);
            //    if (namesId.Count != 0)
            //        idCollection[id.GetHashCode()] = namesId;
            //    else idCollection.Remove(id.GetHashCode());

            //    var idNames = namesCollection[name.GetHashCode()];
            //    idNames.Remove(id);
            //    if (idNames.Count != 0)
            //        namesCollection[name.GetHashCode()] = idNames;
            //    else namesCollection.Remove(name.GetHashCode());
            //}
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
                    var currentId = idGenerator.GetId((key, x), out isFirst);
                    if(!isFirst) res.Add(x, valuesCollection[currentId]);
                }
            return res;
        }

        #region constructors
        public DoubleKeyDictionary()
        {
            valuesCollection = new Dictionary<long, TValue>();
            keys = new Keys<TKeyId, TKeyName>();
        }

        public DoubleKeyDictionary(Tuple<TKeyId, TKeyName, TValue> elem)
        {
            valuesCollection = new Dictionary<long, TValue>();
            keys = new Keys<TKeyId, TKeyName>();

            //idCollection.Add(elem.Item1.GetHashCode(), new List<TKeyName>() { elem.Item2 });
            //namesCollection.Add(elem.Item2.GetHashCode(), new List<TKeyId>() { elem.Item1 });
            valuesCollection.Add((elem.Item1, elem.Item2).GetHashCode(), elem.Item3);
        }

        public DoubleKeyDictionary(TKeyId id, TKeyName name, TValue value)
        {
            valuesCollection = new Dictionary<long, TValue>();
            keys = new Keys<TKeyId, TKeyName>();

            //idCollection.Add(id.GetHashCode(), new List<TKeyName>() { name });
            //namesCollection.Add(name.GetHashCode(), new List<TKeyId>() { id });
            valuesCollection.Add((id, name).GetHashCode(), value);
        }
        #endregion
    }
    

    public sealed class Keys<TKeyId, TKeyName>
    {
        private Dictionary<TKeyId, List<TKeyName>> idCollection;
        private Dictionary<TKeyName, List<TKeyId>> namesCollection;

        public bool TryGetValue<TKey, TValue>(TKey key, out List<TValue> value)
        {
            throw new NotImplementedException();
        }

        public TKeyId this[TKeyName keyName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public TKeyName this[TKeyId keyId]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Guid this[TKeyId keyId, TKeyName keyName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
