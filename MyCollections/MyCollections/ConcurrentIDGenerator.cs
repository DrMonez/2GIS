using System;
using System.Collections.Generic;
using System.Text;

namespace MyCollections
{
    internal class ConcurrentIDGenerator<T>
    {
        private Dictionary<T, long> _dictionary = new Dictionary<T, long>();
        private long _number = 0;
        private object _lockobject = new object();

        public long GetId(T key, out bool isFirst)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            lock (_lockobject)
            {
                if (_dictionary.ContainsKey(key))
                {
                    isFirst = false;
                    return _dictionary[key];
                }
                isFirst = true;
                return _dictionary[key] = _number++;
            }
        }

        public void Remove(T key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            lock (_lockobject)
            {
                if (_dictionary.ContainsKey(key))
                {
                    _dictionary.Remove(key);
                }
            }
        }
    }
}
