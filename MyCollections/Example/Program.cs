using MyCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        class UserType : IComparable<UserType>
        {
            public string Value;
            public UserType(string value)
            {
                Value = value;
            }

            public int CompareTo(UserType other)
            {
                return string.Compare(Value, other.Value);
            }
        }


        static void Main(string[] args)
        { 
            var dictionary = new DoubleKeyDictionary<UserType, int, double>();
            for (var i = 0; i < 10; i++)
                dictionary.TryAdd(new Tuple<UserType, int, double>(new UserType(i.ToString()), i, Math.Pow(-1, i) * i));
            var third = dictionary.GetById(new UserType("3"));
            Console.WriteLine(third.ToString());
            Console.ReadLine();
        }
    }
}
