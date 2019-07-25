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
}
