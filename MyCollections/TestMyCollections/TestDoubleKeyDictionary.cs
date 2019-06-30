using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCollections;

namespace TestMyCollections
{
    [TestClass]
    public class TestDoubleKeyDictionary
    {
        [TestMethod]
        public void AddTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            Assert.AreEqual(1, a.Count);
        }

        [TestMethod]
        public void AddSameObjectsTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            Assert.AreEqual(1, a.Count);
        }

        [TestMethod]
        public void AddSameIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(0, "1", 0.5));
            a.TryAdd(new Tuple<int, string, double>(0, "2", 0.5));
            a.TryAdd(new Tuple<int, string, double>(0, "3", 0.5));
            Assert.AreEqual(4, a.Count);
        }

        [TestMethod]
        public void AddSameNamesTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(1, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(2, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(3, "0", 0.5));
            Assert.AreEqual(4, a.Count);
        }

        [TestMethod]
        public void GetIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            var expected = new Tuple<string, double>[] { new Tuple<string, double>("0", 0.5) };
            var actual = a.GetById(0);
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void GetSameIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(0, "1", 0.5));
            a.TryAdd(new Tuple<int, string, double>(0, "2", 0.5));
            a.TryAdd(new Tuple<int, string, double>(0, "3", 0.5));
            var expected = new Tuple<string, double>[] 
            {
                new Tuple<string, double>("0", 0.5),
                new Tuple<string, double>("1", 0.5),
                new Tuple<string, double>("2", 0.5),
                new Tuple<string, double>("3", 0.5)
            };
            var actual = a.GetById(0);
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void GetNameTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            var expected = new Tuple<int, double>[] { new Tuple<int, double>(0, 0.5) };
            var actual = a.GetByName("0");
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void GetSameNameTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(1, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(2, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(3, "0", 0.5));
            var expected = new Tuple<int, double>[] 
            {
                new Tuple<int, double>(0, 0.5),
                new Tuple<int, double>(1, 0.5),
                new Tuple<int, double>(2, 0.5),
                new Tuple<int, double>(3, 0.5)
            };
            var actual = a.GetByName("0");
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void GetNullTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.Remove(0, "0");
            
            var actual = a.GetByName("0");
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void GetByIndexOperator()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            var expected = 0.5;
            var actual = a[0, "0"];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.Clear();
            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveOneTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));

            a.Remove(0, "0");

            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(1, "2", 0.7));
            a.TryAdd(new Tuple<int, string, double>(1, "1", 0.6));
            a.TryAdd(new Tuple<int, string, double>(3, "3", 0.8));
            a.TryAdd(new Tuple<int, string, double>(4, "4", 0.9));

            a.Remove(1, "1");

            var expectedCount = 4;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

            var elem = a.GetByName("0");
            Assert.AreEqual(1, elem.Length);
            Assert.AreEqual(new Tuple<int, double>(0, 0.5), elem[0]);

            elem = a.GetByName("1");
            Assert.AreEqual(null, elem);

            elem = a.GetByName("2");
            Assert.AreEqual(1, elem.Length);
            Assert.AreEqual(new Tuple<int, double>(1, 0.7), elem[0]);

            elem = a.GetByName("3");
            Assert.AreEqual(1, elem.Length);
            Assert.AreEqual(new Tuple<int, double>(3, 0.8), elem[0]);

            elem = a.GetByName("4");
            Assert.AreEqual(1, elem.Length);
            Assert.AreEqual(new Tuple<int, double>(4, 0.9), elem[0]);
        }
    }


    [TestClass]
    public class TestDoubleKeyDictionaryWithUserType
    {
        [TestMethod]
        public void AddTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            Assert.AreEqual(1, a.Count);
        }

        [TestMethod]
        public void AddSameObjectsTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            Assert.AreEqual(1, a.Count);                                 
        }

        [TestMethod]
        public void AddSameIdTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "1", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "2", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "3", 0.5));
            Assert.AreEqual(4, a.Count);
        }

        [TestMethod]
        public void AddSameNamesTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("1"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("2"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("3"), "0", 0.5));
            Assert.AreEqual(4, a.Count);
        }

        [TestMethod]
        public void GetIdTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            var expected = new Tuple<string, double>[] { new Tuple<string, double>("0", 0.5) };
            var actual = a.GetById(new UserType("0"));
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void GetSameIdTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "1", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "2", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "3", 0.5));
            var expected = new Tuple<string, double>[]
            {
                new Tuple<string, double>("0", 0.5),
                new Tuple<string, double>("1", 0.5),
                new Tuple<string, double>("2", 0.5),
                new Tuple<string, double>("3", 0.5)
            };
            var actual = a.GetById(new UserType("0"));
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void GetNameTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            var expected = new Tuple<UserType, double>[] { new Tuple<UserType, double>(new UserType("0"), 0.5) };
            var actual = a.GetByName("0");
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < actual.Length; i++)
            {
                Assert.IsTrue(expected[i].Item1.Value == actual[i].Item1.Value);
                Assert.AreEqual(expected[i].Item2, actual[i].Item2);
            }
        }

        [TestMethod]
        public void GetSameNameTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("1"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("2"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("3"), "0", 0.5));
            var expected = new Tuple<UserType, double>[]
            {
                new Tuple<UserType, double>(new UserType("0"), 0.5),
                new Tuple<UserType, double>(new UserType("1"), 0.5),
                new Tuple<UserType, double>(new UserType("2"), 0.5),
                new Tuple<UserType, double>(new UserType("3"), 0.5)
            };
            var actual = a.GetByName("0");
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < actual.Length; i++)
            {
                Assert.IsTrue(expected[i].Item1.Value == actual[i].Item1.Value);
                Assert.AreEqual(expected[i].Item2, actual[i].Item2);
            }
        }

        [TestMethod]
        public void GetNullTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            a.Remove(new UserType("0"), "0");

            var actual = a.GetByName("0");
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void GetByIndexOperator()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            var expected = 0.5;
            var actual = a[new UserType("0"), "0"];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            a.Clear();
            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveOneTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));

            a.Remove(new UserType("0"), "0");

            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("0"), "0", 0.5));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("1"), "2", 0.7));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("1"), "1", 0.6));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("3"), "3", 0.8));
            a.TryAdd(new Tuple<UserType, string, double>(new UserType("4"), "4", 0.9));

            a.Remove(new UserType("1"), "1");

            var expectedCount = 4;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

            var elem = a.GetByName("0");
            Assert.AreEqual(1, elem.Length);
            Assert.IsTrue("0" == elem[0].Item1.Value);
            Assert.AreEqual(0.5, elem[0].Item2);

            elem = a.GetByName("1");
            Assert.AreEqual(null, elem);

            elem = a.GetByName("2");
            Assert.AreEqual(1, elem.Length);
            Assert.IsTrue("1" == elem[0].Item1.Value);
            Assert.AreEqual(0.7, elem[0].Item2);

            elem = a.GetByName("3");
            Assert.AreEqual(1, elem.Length);
            Assert.IsTrue("3" == elem[0].Item1.Value);
            Assert.AreEqual(0.8, elem[0].Item2);

            elem = a.GetByName("4");
            Assert.AreEqual(1, elem.Length);
            Assert.IsTrue("4" == elem[0].Item1.Value);
            Assert.AreEqual(0.9, elem[0].Item2);
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
        }
    }
}
