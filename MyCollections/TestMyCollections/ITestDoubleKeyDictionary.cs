using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMyCollections
{
    internal interface ITestDoubleKeyDictionary
    {
        void IdKeysTest();
        void NameKeysTest();
        void ValuesTest();
        void AddTest();
        void AddSameTypesTest();
        void AddSameObjectsTest();
        void AddSameIdTest();
        void AddSameNamesTest();
        void GetIdTest();
        void GetSameIdTest();
        void GetNameTest();
        void GetSameNameTest();
        void GetNullTest();
        void ClearTest();
        void RemoveOneTest();
        void RemoveTest();
    }
}
