using System.Collections.Generic;

namespace MyCollections
{
    public class IDGenerator
    {
        Dictionary<object, long> d = new Dictionary<object, long>();
        long gid = 0;

        public long GetId(object o, out bool isFirst)
        {
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
