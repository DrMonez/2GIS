using System;
using System.Collections.Generic;

namespace MyCollections
{
    internal class IDGenerator
    {
        private Dictionary<object, long> _dictionary = new Dictionary<object, long>();
        private long _number = 0;

        public long GetId(object key, out bool isFirst)
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

        public void Remove(object key)
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

    internal class ConcurrentIDGenerator
    {
        private Dictionary<object, long> _dictionary = new Dictionary<object, long>();
        private long _number = 0;
        private object _lockobject = new object();

        public long GetId(object key, out bool isFirst)
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

        public void Remove(object key)
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
