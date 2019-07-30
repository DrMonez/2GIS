using System;
using System.Collections.Generic;

namespace MyCollections
{
    internal class IDGenerator
    {
        Dictionary<object, long> d = new Dictionary<object, long>();
        long gid = 0;

        public long GetId(object o, out bool isFirst)
        {
            if(o == null)
            {
                throw new ArgumentNullException("0");
            }

            if (d.ContainsKey(o))
            {
                isFirst = false;
                return d[o];
            }
            isFirst = true;
            return d[o] = gid++;
        }
    }

    internal class ConcurrentIDGenerator
    {
        private Dictionary<object, long> _dictionary = new Dictionary<object, long>();
        private long _number = 0;
        private object _lockobject = new object();

        public long GetId(object o, out bool isFirst)
        {
            if (o == null)
            {
                throw new ArgumentNullException("0");
            }
            lock (_lockobject)
            {
                if (_dictionary.ContainsKey(o))
                {
                    isFirst = false;
                    return _dictionary[o];
                }
                isFirst = true;
                return _dictionary[o] = _number++;
            }
        }
    }
}
