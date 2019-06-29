using System;
using System.Collections;
using System.Collections.Generic;

namespace MyCollections
{
    public class DoubleKeyDictionary<TKeyId, TKeyName, TValue> : IDoubleKeyDictionary<TKeyId, TKeyName, TValue>
    {
        private Dictionary<TKeyId, TKeyName> identifiers;
        private Dictionary<TKeyName, TKeyId> names;
        private Hashtable values;
        private object locker = new object();

        public int Count => values.Count;

        public bool TryAdd(TKeyId id, TKeyName name, TValue value)
        {
            return TryAdd(new Tuple<TKeyId, TKeyName, TValue>(id, name, value));
        }

        public bool TryAdd(Tuple<TKeyId, TKeyName, TValue> elem)
        {
            lock (locker)
            {
                if (!ContainsDoubleKey(elem.Item1, elem.Item2))
                {
                    values.Add((elem.Item1, elem.Item2), elem.Item3);
                    identifiers.Add(elem.Item1, elem.Item2);
                    names.Add(elem.Item2, elem.Item1);
                }
                else return false;
                return true;
            }
        }

        private bool ContainsDoubleKey(TKeyId id, TKeyName name)
        {
            return this.identifiers.ContainsKey(id) || names.ContainsKey(name);
        }

        public void Clear()
        {
            lock (locker)
            {
                identifiers.Clear();
                names.Clear();
                values.Clear();
            }
        }

        public void Remove(TKeyId id)
        {
            TKeyName name;
            lock (locker)
            {
                if (this.identifiers.TryGetValue(id, out name))
                {
                    values.Remove((id, name));
                    this.identifiers.Remove(id);
                    names.Remove(name);
                }
            }
        }

        public void Remove(TKeyName name)
        {
            TKeyId id;
            lock (locker)
            {
                if (names.TryGetValue(name, out id))
                {
                    values.Remove((id, name));
                    this.identifiers.Remove(id);
                    names.Remove(name);
                }
            }
        }
        
        public Tuple<TKeyName, TValue> GetById(TKeyId id)
        {
            TKeyName name;
            if (this.identifiers.TryGetValue(id, out name))
                return new Tuple<TKeyName, TValue>(name, (TValue)values[(id, name)]);
            else return null;
        }

        public Tuple<TKeyId, TValue> GetByName(TKeyName name)
        {
            TKeyId id;
            if (this.names.TryGetValue(name, out id))
                return new Tuple<TKeyId, TValue>(id, (TValue)values[(id, name)]);
            else return null;
        }

        #region constructors
        public DoubleKeyDictionary()
        {
            //elements = new List<TValue>();

            values = new Hashtable();
            identifiers = new Dictionary<TKeyId, TKeyName>();
            names = new Dictionary<TKeyName, TKeyId>();
        }

        public DoubleKeyDictionary(Tuple<TKeyId, TKeyName, TValue> elem)
        {
            values = new Hashtable();
            identifiers = new Dictionary<TKeyId, TKeyName>();
            names = new Dictionary<TKeyName, TKeyId>();
            
            identifiers.Add(elem.Item1, elem.Item2);
            names.Add(elem.Item2, elem.Item1);
            values.Add((elem.Item1, elem.Item2), elem.Item3);
        }

        public DoubleKeyDictionary(TKeyId id, TKeyName name, TValue value)
        {
            this.values = new Hashtable();
            this.identifiers = new Dictionary<TKeyId, TKeyName>();
            names = new Dictionary<TKeyName, TKeyId>();
            
            this.identifiers.Add(id, name);
            names.Add(name, id);
            values.Add((id, name), value);
        }
        #endregion

        class TreeNode<TKey>
        {
            private readonly TKey id;
            private int index;

            public TKey Id => id;
            public int Index
            {
                get { return index; }
                set { index = value; }
            }

            public TreeNode<TKey> Left;
            public TreeNode<TKey> Right;
            public TreeNode<TKey> Parent;


            public TreeNode(Tuple<TKey, int> elem)
            {
                id = elem.Item1;
                index = elem.Item2;
            }
        }
    }
}
