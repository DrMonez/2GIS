using System;
using System.Collections.Generic;

namespace MyCollections
{
    internal class IDGenerator<T>
    {
        private Dictionary<T, long> _dictionary = new Dictionary<T, long>();
        private long _number = 0;

        public long GetId(T key, out bool isFirst)
        {
            if(key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (_dictionary.ContainsKey(key))
            {
                isFirst = false;
                return _dictionary[key];
            }
            isFirst = true;
            return _dictionary[key] = _number++;
        }

        public void Remove(T key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (_dictionary.ContainsKey(key))
            {
                _dictionary.Remove(key);
            }
        }
    }
}
