using System;
using System.Collections.Generic;
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
            a.Add(0, "0", 0.0);
            Assert.AreEqual(1, a.Count);
        }

        [TestMethod]
        public void AddSameObjectsTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.0);
            a.Add(0, "0", 0.0);
            a.Add(0, "0", 0.0);
            a.Add(0, "0", 0.0);
            Assert.AreEqual(1, a.Count);
        }

        [TestMethod]
        public void AddSameIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.5);
            a.Add(0, "1", 0.5);
            a.Add(0, "2", 0.5);
            a.Add(0, "3", 0.5);
            Assert.AreEqual(4, a.Count);
        }

        [TestMethod]
        public void AddSameNamesTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.5);
            a.Add(1, "0", 0.5);
            a.Add(2, "0", 0.5);
            a.Add(3, "0", 0.5);
            Assert.AreEqual(4, a.Count);
        }

        [TestMethod]
        public void GetIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.5);
            var expected = new Tuple<string, double>[] { new Tuple<string, double>("0", 0.5) };
            var actual = a.GetById(0);
            Assert.AreEqual(expected.Length, actual.Count);
            var keys = new string[1];
            actual.Keys.CopyTo(keys, 0);
            var values = new double[1];
            actual.Values.CopyTo(values, 0);
            for (var i = 0; i < keys.Length; i++)
            {
                Assert.AreEqual(expected[i].Item1, keys[i]);
                Assert.AreEqual(expected[i].Item2, values[i]);
            }
        }

        [TestMethod]
        public void GetSameIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.5);
            a.Add(0, "1", 0.5);
            a.Add(0, "2", 0.5);
            a.Add(0, "3", 0.5);
            var expected = new Dictionary<string, double>()
            {
                { "0", 0.5 },
                { "1", 0.5 },
                { "2", 0.5 },
                { "3", 0.5 }
            };
            var actual = a.GetById(0);
            Assert.AreEqual(expected.Count, actual.Count);
            var keys = new string[4];
            actual.Keys.CopyTo(keys, 0);
            for (var i = 0; i < actual.Count; i++)
                Assert.AreEqual(expected[keys[i]], actual[keys[i]]);
        }

        [TestMethod]
        public void GetNameTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.5);
            var expected = new Dictionary<int, double>() { { 0, 0.5 } };
            var actual = a.GetByName("0");
            Assert.AreEqual(expected.Count, actual.Count);
            var keys = new int[1];
            actual.Keys.CopyTo(keys, 0);
            for (var i = 0; i < actual.Count; i++)
                Assert.AreEqual(expected[keys[i]], actual[keys[i]]);
        }

        [TestMethod]
        public void GetSameNameTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.5);
            a.Add(1, "0", 0.5);
            a.Add(2, "0", 0.5);
            a.Add(3, "0", 0.5);
            var expected = new Dictionary<int, double>()
            {
                { 0, 0.5 },
                { 1, 0.5 },
                { 2, 0.5 },
                { 3, 0.5 }
            };
            var actual = a.GetByName("0");
            Assert.AreEqual(expected.Count, actual.Count);
            var keys = new int[4];
            actual.Keys.CopyTo(keys, 0);
            for (var i = 0; i < actual.Count; i++)
                Assert.AreEqual(expected[keys[i]], actual[keys[i]]);
        }

        [TestMethod]
        public void GetNullTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.5);
            a.Remove(0, "0");
            try
            {
                a.GetByName("0");
            }
            catch(Exception ex)
            {
                Assert.IsTrue(ex is KeyNotFoundException);
            }
        }

        [TestMethod]
        public void GetByIndexOperator()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.5);
            var expected = 0.5;
            var actual = a[0, "0"];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.5);
            a.Clear();
            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveOneTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.5);

            a.Remove(0, "0");

            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(0, "0", 0.5);
            a.Add(1, "2", 0.7);
            a.Add(1, "1", 0.6);
            a.Add(3, "3", 0.8);
            a.Add(4, "4", 0.9);

            a.Remove(1, "1");

            var expectedCount = 4;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

            var elem = a.GetByName("0");
            Assert.AreEqual(1, elem.Count);
            Assert.AreEqual( 0.5, elem[0]);
            
            try
            {
                a.GetByName("1");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is KeyNotFoundException);
            }

            elem = a.GetByName("2");
            Assert.AreEqual(1, elem.Count);
            Assert.AreEqual(0.7, elem[1]);

            elem = a.GetByName("3");
            Assert.AreEqual(1, elem.Count);
            Assert.AreEqual(0.8, elem[3]);

            elem = a.GetByName("4");
            Assert.AreEqual(1, elem.Count);
            Assert.AreEqual(0.9, elem[4]);
        }
    }




    [TestClass]
    public class TestDoubleKeyDictionaryWithUserType
    {
        [TestMethod]
        public void AddTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.0);
            Assert.AreEqual(1, a.Count);
        }

        [TestMethod]
        public void AddSameObjectsTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.0);
            a.Add(new UserType("0"), "0", 0.0);
            a.Add(new UserType("0"), "0", 0.0);
            a.Add(new UserType("0"), "0", 0.0);
            Assert.AreEqual(1, a.Count);
        }

        [TestMethod]
        public void AddSameIdTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.5);
            a.Add(new UserType("0"), "1", 0.5);
            a.Add(new UserType("0"), "2", 0.5);
            a.Add(new UserType("0"), "3", 0.5);
            Assert.AreEqual(4, a.Count);
        }

        [TestMethod]
        public void AddSameNamesTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.5);
            a.Add(new UserType("1"), "0", 0.5);
            a.Add(new UserType("2"), "0", 0.5);
            a.Add(new UserType("3"), "0", 0.5);
            Assert.AreEqual(4, a.Count);
        }

        [TestMethod]
        public void GetIdTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.5);
            var expected = new Tuple<string, double>[] { new Tuple<string, double>("0", 0.5) };
            var actual = a.GetById(new UserType("0"));
            Assert.AreEqual(expected.Length, actual.Count);
            var keys = new string[1];
            actual.Keys.CopyTo(keys, 0);
            var values = new double[1];
            actual.Values.CopyTo(values, 0);
            for (var i = 0; i < keys.Length; i++)
            {
                Assert.AreEqual(expected[i].Item1, keys[i]);
                Assert.AreEqual(expected[i].Item2, values[i]);
            }
        }

        [TestMethod]
        public void GetSameIdTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.5);
            a.Add(new UserType("0"), "1", 0.5);
            a.Add(new UserType("0"), "2", 0.5);
            a.Add(new UserType("0"), "3", 0.5);
            var expected = new Dictionary<string, double>()
            {
                { "0", 0.5 },
                { "1", 0.5 },
                { "2", 0.5 },
                { "3", 0.5 }
            };
            var actual = a.GetById(new UserType("0"));
            Assert.AreEqual(expected.Count, actual.Count);
            var keys = new string[4];
            actual.Keys.CopyTo(keys, 0);
            for (var i = 0; i < actual.Count; i++)
                Assert.AreEqual(expected[keys[i]], actual[keys[i]]);
        }

        [TestMethod]
        public void GetNameTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.5);
            var expected = new Dictionary<UserType, double>() { { new UserType("0"), 0.5 } };
            var actual = a.GetByName("0");
            Assert.AreEqual(expected.Count, actual.Count);
            var keys = new UserType[1];
            actual.Keys.CopyTo(keys, 0);
            for (var i = 0; i < actual.Count; i++)
                Assert.AreEqual(expected[keys[i]], actual[keys[i]]);
        }

        [TestMethod]
        public void GetSameNameTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.5);
            a.Add(new UserType("1"), "0", 0.5);
            a.Add(new UserType("2"), "0", 0.5);
            a.Add(new UserType("3"), "0", 0.5);
            var expected = new Dictionary<UserType, double>()
            {
                { new UserType("0"), 0.5 },
                { new UserType("1"), 0.5 },
                { new UserType("2"), 0.5 },
                { new UserType("3"), 0.5 }
            };
            var actual = a.GetByName("0");
            Assert.AreEqual(expected.Count, actual.Count);
            var keys = new UserType[4];
            actual.Keys.CopyTo(keys, 0);
            for (var i = 0; i < actual.Count; i++)
                Assert.AreEqual(expected[keys[i]], actual[keys[i]]);
        }

        [TestMethod]
        public void GetNullTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.5);
            a.Remove(new UserType("0"), "0");
            try
            {
                a.GetByName("0");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is KeyNotFoundException);
            }
        }

        [TestMethod]
        public void GetByIndexOperator()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.5);
            var expected = 0.5;
            var actual = a[new UserType("0"), "0"];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.5);
            a.Clear();
            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveOneTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.5);

            a.Remove(new UserType("0"), "0");

            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var a = new DoubleKeyDictionary<UserType, string, double>();
            a.Add(new UserType("0"), "0", 0.5);
            a.Add(new UserType("1"), "2", 0.7);
            a.Add(new UserType("1"), "1", 0.6);
            a.Add(new UserType("3"), "3", 0.8);
            a.Add(new UserType("4"), "4", 0.9);

            a.Remove(new UserType("1"), "1");

            var expectedCount = 4;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

            var elem = a.GetByName("0");
            Assert.AreEqual(1, elem.Count);
            Assert.AreEqual(0.5, elem[new UserType("0")]);

            try
            {
                a.GetByName("1");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is KeyNotFoundException);
            }

            elem = a.GetByName("2");
            Assert.AreEqual(1, elem.Count);
            Assert.AreEqual(0.7, elem[new UserType("1")]);

            elem = a.GetByName("3");
            Assert.AreEqual(1, elem.Count);
            Assert.AreEqual(0.8, elem[new UserType("3")]);

            elem = a.GetByName("4");
            Assert.AreEqual(1, elem.Count);
            Assert.AreEqual(0.9, elem[new UserType("4")]);
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
