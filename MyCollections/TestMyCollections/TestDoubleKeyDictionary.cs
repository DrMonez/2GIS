using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCollections;

namespace TestMyCollections
{
    [TestClass]
    public class TestDoubleKeyDictionary : ITestDoubleKeyDictionary
    {
        [TestMethod]
        public void IdKeysTest()
        {
            var count = 3;
            var doubleKeyDictionary = initialize<int, string, double>(count);

            var expected = new List<int>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i);
            }

            var actual = doubleKeyDictionary.IdKeys.ToList();

            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void NameKeysTest()
        {
            var count = 3;
            var doubleKeyDictionary = initialize<string, int, double>(count);

            var expected = new List<int>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i);
            }

            var actual = doubleKeyDictionary.NameKeys.ToList();

            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void ValuesTest()
        {
            var count = 3;
            var doubleKeyDictionary = initialize<string, int, double>(count);

            var expected = new List<double>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i);
            }

            var actual = doubleKeyDictionary.Values.ToList();

            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void AddTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);

            var expected = count;

            var actual = doubleKeyDictionary.Count;
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddSameTypesTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, int, double>(count);

            var expected = count;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddSameObjectsTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);
            Assert.ThrowsException<ArgumentException>(() => doubleKeyDictionary.Add(0, "0", 0.0));
        }

        [TestMethod]
        public void AddSameIdTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);
            for (var i = 1; i < 4; i++) 
            {
                doubleKeyDictionary.Add(0, i.ToString(), 0.5);
            }

            var expected = 4;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddSameNamesTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);
            for (var i = 1; i < 4; i++)
            {
                doubleKeyDictionary.Add(i, "0", 0.5);
            }

            var expected = 4;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetIdTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);

            var expected = new Dictionary<string, double>();
            for(var i = 0; i < count; i++)
            {
                expected.Add(i.ToString(), i);
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();

            var actual = doubleKeyDictionary.GetById(0);
            var actualKeys = actual.Keys.ToList();
            var actualValues = actual.Values.ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedKeys[i], actualKeys[i]);
                Assert.AreEqual(expectedValues[i], actualValues[i]);
            }
        }

        [TestMethod]
        public void GetSameIdTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);
            count = 3;
            for (var i = 1; i < count; i++)
            {
                doubleKeyDictionary.Add(0, i.ToString(), i);
            }

            var expected = new Dictionary<string, double>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i.ToString(), i);
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();

            var actual = doubleKeyDictionary.GetById(0);
            var actualKeys = actual.Keys.ToList();
            var actualValues = actual.Values.ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedKeys[i], actualKeys[i]);
                Assert.AreEqual(expectedValues[i], actualValues[i]);
            }
        }

        [TestMethod]
        public void GetNameTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);

            var expected = new Dictionary<int, double>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i, i);
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();

            var actual = doubleKeyDictionary.GetByName("0");
            var actualKeys = actual.Keys.ToList();
            var actualValues = actual.Values.ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedKeys[i], actualKeys[i]);
                Assert.AreEqual(expectedValues[i], actualValues[i]);
            }
        }

        [TestMethod]
        public void GetSameNameTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);
            count = 3;
            for (var i = 1; i < count; i++)
            {
                doubleKeyDictionary.Add(i, "0", i);
            }

            var expected = new Dictionary<int, double>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i, i);
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();

            var actual = doubleKeyDictionary.GetByName("0");
            var actualKeys = actual.Keys.ToList();
            var actualValues = actual.Values.ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedKeys[i], actualKeys[i]);
                Assert.AreEqual(expectedValues[i], actualValues[i]);
            }
        }

        [TestMethod]
        public void GetNullTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);
            doubleKeyDictionary.Remove(0, "0");

            var expected = 0;

            var actual = doubleKeyDictionary.GetByName("0").Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetByIndexOperator()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);

            var expected = 0;

            var actual = doubleKeyDictionary[0, "0"];

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);
            doubleKeyDictionary.Clear();

            var expected = 0;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveOneTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<int, string, double>(count);
            doubleKeyDictionary.Remove(0, "0");

            var expected = 0;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var count = 4;
            var doubleKeyDictionary = initialize<int, string, double>(count);
            doubleKeyDictionary.Add(1, "2", 0.7);
            doubleKeyDictionary.Remove(1, "1");

            var expectedCount = 4;
            var actualCount = doubleKeyDictionary.Count;
            Assert.AreEqual(expectedCount, actualCount);

            expectedCount = 2;
            actualCount = doubleKeyDictionary.GetByName("2").Count;
            Assert.AreEqual(expectedCount, actualCount);

            expectedCount = 0;
            actualCount = doubleKeyDictionary.GetByName("1").Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        private DoubleKeyDictionary<TKeyId, TKeyName, TValue> initialize<TKeyId, TKeyName, TValue>(int count)
        {
            var doubleKeyDictionary = new DoubleKeyDictionary<TKeyId, TKeyName, TValue>();
            var typeId = Nullable.GetUnderlyingType(typeof(TKeyId)) ?? typeof(TKeyId);
            var typeName = Nullable.GetUnderlyingType(typeof(TKeyName)) ?? typeof(TKeyName);
            var typeValue = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            for (var i = 0; i < count; i++) 
            {
                doubleKeyDictionary.Add((TKeyId)Convert.ChangeType(i, typeId, CultureInfo.InvariantCulture), (TKeyName)Convert.ChangeType(i, typeName, CultureInfo.InvariantCulture), (TValue)Convert.ChangeType(i, typeValue, CultureInfo.InvariantCulture));
            }
            return doubleKeyDictionary;
        }
    }




    [TestClass]
    public class TestDoubleKeyDictionaryWithUserType : ITestDoubleKeyDictionary
    {
        public void IdKeysTest()
        {
            var count = 3;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);

            var expected = new List<UserType>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(new UserType(i.ToString()));
            }

            var actual = doubleKeyDictionary.IdKeys.ToList();

            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void NameKeysTest()
        {
            var count = 3;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);

            var expected = new List<string>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i.ToString());
            }

            var actual = doubleKeyDictionary.NameKeys.ToList();

            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void ValuesTest()
        {
            var count = 3;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);

            var expected = new List<string>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i.ToString());
            }

            var actual = doubleKeyDictionary.Values.ToList();

            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void AddTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);

            var expected = count;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddSameTypesTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, UserType, string>(count);

            var expected = count;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddSameObjectsTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);
            Assert.ThrowsException<ArgumentException>(() => doubleKeyDictionary.Add(new UserType("0"), "0", "0"));
        }

        [TestMethod]
        public void AddSameIdTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);
            for (var i = 1; i < 4; i++)
            {
                doubleKeyDictionary.Add(new UserType("0"), i.ToString(), i.ToString());
            }

            var expected = 4;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddSameNamesTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);
            for (var i = 1; i < 4; i++)
            {
                doubleKeyDictionary.Add(new UserType(i.ToString()), "0", i.ToString());
            }

            var expected = 4;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetIdTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);

            var expected = new Dictionary<string, string>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i.ToString(), i.ToString());
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();

            var actual = doubleKeyDictionary.GetById(new UserType("0"));
            var actualKeys = actual.Keys.ToList();
            var actualValues = actual.Values.ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedKeys[i], actualKeys[i]);
                Assert.AreEqual(expectedValues[i], actualValues[i]);
            }
        }

        [TestMethod]
        public void GetSameIdTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);
            count = 3;
            for (var i = 1; i < count; i++)
            {
                doubleKeyDictionary.Add(new UserType("0"), i.ToString(), i.ToString());
            }

            var expected = new Dictionary<string, string>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i.ToString(), i.ToString());
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();

            var actual = doubleKeyDictionary.GetById(new UserType("0"));
            var actualKeys = actual.Keys.ToList();
            var actualValues = actual.Values.ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedKeys[i], actualKeys[i]);
                Assert.AreEqual(expectedValues[i], actualValues[i]);
            }
        }

        [TestMethod]
        public void GetNameTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);

            var expected = new Dictionary<UserType, string>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(new UserType(i.ToString()), i.ToString());
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();

            var actual = doubleKeyDictionary.GetByName("0");
            var actualKeys = actual.Keys.ToList();
            var actualValues = actual.Values.ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedKeys[i], actualKeys[i]);
                Assert.AreEqual(expectedValues[i], actualValues[i]);
            }
        }

        [TestMethod]
        public void GetSameNameTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);
            count = 3;
            for (var i = 1; i < count; i++)
            {
                doubleKeyDictionary.Add(new UserType(i.ToString()), "0", i.ToString());
            }

            var expected = new Dictionary<UserType, string>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(new UserType(i.ToString()), i.ToString());
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();

            var actual = doubleKeyDictionary.GetByName("0");
            var actualKeys = actual.Keys.ToList();
            var actualValues = actual.Values.ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedKeys[i], actualKeys[i]);
                Assert.AreEqual(expectedValues[i], actualValues[i]);
            }
        }

        [TestMethod]
        public void GetNullTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);
            doubleKeyDictionary.Remove(new UserType("0"), "0");

            var expected = 0;

            var actual = doubleKeyDictionary.GetByName("0").Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetByIndexOperator()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);

            var expected = "0";

            var actual = doubleKeyDictionary[new UserType("0"), "0"];

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);
            doubleKeyDictionary.Clear();

            var expected = 0;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveOneTest()
        {
            var count = 1;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);
            doubleKeyDictionary.Remove(new UserType("0"), "0");

            var expected = 0;

            var actual = doubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var count = 4;
            var doubleKeyDictionary = initialize<UserType, string, string>(count);
            doubleKeyDictionary.Add(new UserType("1"), "2", "0.7");
            doubleKeyDictionary.Remove(new UserType("1"), "1");

            var expectedCount = 4;
            var actualCount = doubleKeyDictionary.Count;
            Assert.AreEqual(expectedCount, actualCount);

            expectedCount = 2;
            actualCount = doubleKeyDictionary.GetByName("2").Count;
            Assert.AreEqual(expectedCount, actualCount);

            expectedCount = 0;
            actualCount = doubleKeyDictionary.GetByName("1").Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        private DoubleKeyDictionary<TKeyId, TKeyName, TValue> initialize<TKeyId, TKeyName, TValue>(int count)   where TKeyId : class 
                                                                                                                where TKeyName : class
                                                                                                                where TValue : class
        {
            var doubleKeyDictionary = new DoubleKeyDictionary<TKeyId, TKeyName, TValue>();

            var typeId = Nullable.GetUnderlyingType(typeof(TKeyId)) ?? typeof(TKeyId);
            var typeName = Nullable.GetUnderlyingType(typeof(TKeyName)) ?? typeof(TKeyName);
            var typeValue = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

            var idIsUserType = typeId == typeof(UserType);
            var nameIsUserType = typeName == typeof(UserType);
            var valueIsUserType = typeValue == typeof(UserType);

            for (var i = 0; i < count; i++)
            {
                var id = idIsUserType ? new UserType(i.ToString()) as TKeyId : (TKeyId)Convert.ChangeType(i, typeId, CultureInfo.InvariantCulture);
                var name = nameIsUserType ? new UserType(i.ToString()) as TKeyName : (TKeyName)Convert.ChangeType(i, typeName, CultureInfo.InvariantCulture);
                var value = valueIsUserType ? new UserType(i.ToString()) as TValue : (TValue)Convert.ChangeType(i, typeValue, CultureInfo.InvariantCulture);
                doubleKeyDictionary.Add(id, name, value);
            }
            return doubleKeyDictionary;
        }

        public class UserType : IEquatable<UserType>
        {
            public string Value;
            public UserType(string value)
            {
                Value = value;
            }
            public override int GetHashCode() => Value.GetHashCode();
            public bool Equals(UserType other) => Value.Equals(other.Value);

            public override bool Equals(object obj)
            {
                var res = false;
                try
                {
                    res = Value.Equals(((UserType)obj).Value);
                }
                catch { res = false; }
                return res;
            }
        }
    }
}
