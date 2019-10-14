using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCollections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestMyCollections
{
    [TestClass]
    public class TestConcurrentDoubleKeyDictionary : ITestDoubleKeyDictionary
    {
        [TestMethod]
        public void AddTest()
        {
            var count = 100000;
            var concurrentDoubleKeyDictionary = Initialize<string, int, double>(count);

            var expected = count;

            var actual = concurrentDoubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddSameTypesTest()
        {
            var count = 1;
            var concurrentDoubleKeyDictionary = Initialize<int, int, double>(count);

            var expected = count;

            var actual = concurrentDoubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddSameObjectsTest()
        {
            var count = 1;
            var concurrentDoubleKeyDictionary = Initialize<string, int, double>(count);

            var actual = concurrentDoubleKeyDictionary.TryAdd("0", 0, 0);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void AddSameIdTest()
        {
            var count = 1;
            var concurrentDoubleKeyDictionary = Initialize<int, string, double>(count);
            count = 100001;

            Parallel.For(1, count, i =>
            {
                concurrentDoubleKeyDictionary.TryAdd(0, i.ToString(), i);
            });

            var expected = count;

            var actual = concurrentDoubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddSameNamesTest()
        {
            var count = 1;
            var concurrentDoubleKeyDictionary = Initialize<int, string, double>(count);
            count = 100001;

            Parallel.For(1, count, i =>
            {
                concurrentDoubleKeyDictionary.TryAdd(i, "0", i);
            });

            var expected = count;

            var actual = concurrentDoubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IdKeysTest()
        {
            var count = 100000;
            var concurrentDoubleKeyDictionary = Initialize<int, string, double>(count);

            var expected = new List<int>();
            for(var i =0; i<count;i++)
            {
                expected.Add(i);
            }

            var actual = concurrentDoubleKeyDictionary.IdKeys.ToList();

            Assert.AreEqual(expected.Count, actual.Count);

            expected.ForEach(x => {
                Assert.IsTrue(actual.Contains(x));
                actual.Remove(x);
            });

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void NameKeysTest()
        {
            var count = 100000;
            var concurrentDoubleKeyDictionary = Initialize<string, int, double>(count);

            var expected = new List<int>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i);
            }

            var actual = concurrentDoubleKeyDictionary.NameKeys.ToList();

            Assert.AreEqual(expected.Count, actual.Count);

            expected.ForEach(x => {
                Assert.IsTrue(actual.Contains(x));
                actual.Remove(x);
            });

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void ValuesTest()
        {
            var count = 100000;
            var concurrentDoubleKeyDictionary = Initialize<int, string, double>(count);

            var expected = new List<double>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i);
            }

            var actual = concurrentDoubleKeyDictionary.Values.ToList();

            Assert.AreEqual(expected.Count, actual.Count);

            expected.ForEach(x =>
            {
                Assert.IsTrue(actual.Contains(x));
                actual.Remove(x);
            });

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GetIdTest()
        {
            var count = 1;
            var concurrentDoubleKeyDictionary = Initialize<int, string, double>(count);

            var expected = new Dictionary<string, double>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i.ToString(), i);
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();
            
            Assert.IsTrue(concurrentDoubleKeyDictionary.TryGetById(0, out var actual));
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
            var concurrentDoubleKeyDictionary = Initialize<int, string, double>(count);
            count = 3;
            Parallel.For(1, count, i =>
            {
                concurrentDoubleKeyDictionary.TryAdd(0, i.ToString(), i);
            });

            var expected = new Dictionary<string, double>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i.ToString(), i);
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();

            Assert.IsTrue(concurrentDoubleKeyDictionary.TryGetById(0, out var actual));
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
            var concurrentDoubleKeyDictionary = Initialize<int, string, double>(count);

            var expected = new Dictionary<int, double>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i, i);
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();

            Assert.IsTrue(concurrentDoubleKeyDictionary.TryGetByName("0", out var actual));
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
            var concurrentDoubleKeyDictionary = Initialize<int, string, double>(count);
            count = 3;
            Parallel.For(1, count, i =>
            {
                concurrentDoubleKeyDictionary.TryAdd(i, "0", i);
            });

            var expected = new Dictionary<int, double>();
            for (var i = 0; i < count; i++)
            {
                expected.Add(i, i);
            }
            var expectedKeys = expected.Keys.ToList();
            var expectedValues = expected.Values.ToList();

            Assert.IsTrue(concurrentDoubleKeyDictionary.TryGetByName("0", out var actual));
            var actualKeys = actual.Keys.ToList();
            var actualValues = actual.Values.ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < count; i++)
            {
                Assert.IsTrue(actualKeys.Contains(expectedKeys[i]));
                Assert.IsTrue(actualValues.Contains(expectedValues[i]));
            }
        }

        [TestMethod]
        public void GetNullTest()
        {
            var count = 1;
            var concurrentDoubleKeyDictionary = Initialize<int, string, double>(count);
            Assert.IsTrue(concurrentDoubleKeyDictionary.TryRemove(0, "0"));

            var expected = 0;

            Assert.IsTrue(!concurrentDoubleKeyDictionary.TryGetByName("0", out var actualDictionary));
            var actual = actualDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearTest()
        {
            var count = 1;
            var concurrentDoubleKeyDictionary = Initialize<int, string, string>(count);
            concurrentDoubleKeyDictionary.Clear();

            var expected = 0;

            var actual = concurrentDoubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveOneTest()
        {
            var count = 1;
            var concurrentDoubleKeyDictionary = Initialize<int, string, double>(count);
            Assert.IsTrue(concurrentDoubleKeyDictionary.TryRemove(0, "0"));

            var expected = 0;

            var actual = concurrentDoubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var count = 100000;
            var concurrentDoubleKeyDictionary = Initialize<int, string, string>(count);

            Parallel.For(0, count, i =>
            {
                concurrentDoubleKeyDictionary.TryRemove(i, i.ToString());
            });
            count = 0;

            var expected = count;
            var actual = concurrentDoubleKeyDictionary.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveAddTest()
        {
            var count = 10;
            var concurrentDoubleKeyDictionary = Initialize<string, int, double>(count);

            concurrentDoubleKeyDictionary.TryRemove("0", 0);
            concurrentDoubleKeyDictionary.TryAdd("0", 0, 0);

            var expected = count;

            var actual = concurrentDoubleKeyDictionary.Count;

            Assert.AreEqual(expected, actual);
        }

        private ConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue> Initialize<TKeyId, TKeyName, TValue>(int count)
        {
            var concurrentDoubleKeyDictionary = new ConcurrentDoubleKeyDictionary<TKeyId, TKeyName, TValue>();
            var typeId = Nullable.GetUnderlyingType(typeof(TKeyId)) ?? typeof(TKeyId);
            var typeName = Nullable.GetUnderlyingType(typeof(TKeyName)) ?? typeof(TKeyName);
            var typeValue = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            Parallel.For(0, count, i =>
            {
                concurrentDoubleKeyDictionary.TryAdd((TKeyId)Convert.ChangeType(i, typeId, CultureInfo.InvariantCulture), (TKeyName)Convert.ChangeType(i, typeName, CultureInfo.InvariantCulture), (TValue)Convert.ChangeType(i, typeValue, CultureInfo.InvariantCulture));
            });
            return concurrentDoubleKeyDictionary;
        }
    }
}
