using System;
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
        public void GetIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            var expected = new Tuple<string, double>("0", 0.5);
            var actual = a.GetById(0);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetNameTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            var expected = new Tuple<int, double>(0, 0.5);
            var actual = a.GetByName("0");
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
        public void RemoveOneByIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));

            a.Remove(0);

            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveOneByNameTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));

            a.Remove("0");

            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveByIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(2, "2", 0.7));
            a.TryAdd(new Tuple<int, string, double>(1, "1", 0.6));
            a.TryAdd(new Tuple<int, string, double>(3, "3", 0.8));
            a.TryAdd(new Tuple<int, string, double>(4, "4", 0.9));

            a.Remove(1);

            var expectedCount = 4;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

            var elem = a.GetByName("0");
            Assert.AreEqual(new Tuple<int, double>(0, 0.5), elem);

            elem = a.GetByName("1");
            Assert.AreEqual(null, elem);

            elem = a.GetByName("2");
            Assert.AreEqual(new Tuple<int, double>(2, 0.7), elem);

            elem = a.GetByName("3");
            Assert.AreEqual(new Tuple<int, double>(3, 0.8), elem);

            elem = a.GetByName("4");
            Assert.AreEqual(new Tuple<int, double>(4, 0.9), elem);
        }

        [TestMethod]
        public void RemoveByNameTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(2, "2", 0.6));
            a.TryAdd(new Tuple<int, string, double>(1, "1", 0.7));
            a.TryAdd(new Tuple<int, string, double>(3, "3", 0.8));
            a.TryAdd(new Tuple<int, string, double>(4, "4", 0.9));

            a.Remove("1");

            var expectedCount = 4;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

            var elem = a.GetByName("0");
            Assert.AreEqual(new Tuple<int, double>(0, 0.5), elem);

            elem = a.GetByName("1");
            Assert.AreEqual(null, elem);

            elem = a.GetByName("2");
            Assert.AreEqual(new Tuple<int, double>(2, 0.6), elem);

            elem = a.GetByName("3");
            Assert.AreEqual(new Tuple<int, double>(3, 0.8), elem);

            elem = a.GetByName("4");
            Assert.AreEqual(new Tuple<int, double>(4, 0.9), elem);
        }

        [TestMethod]
        public void RemoveLastTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.TryAdd(new Tuple<int, string, double>(0, "0", 0.5));
            a.TryAdd(new Tuple<int, string, double>(1, "1", 0.7));
            a.TryAdd(new Tuple<int, string, double>(2, "2", 0.6));
            a.TryAdd(new Tuple<int, string, double>(3, "3", 0.8));
            a.TryAdd(new Tuple<int, string, double>(4, "4", 0.9));

            a.Remove("4");

            var expectedCount = 4;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

            var elem = a.GetByName("0");
            Assert.AreEqual(new Tuple<int, double>(0, 0.5), elem);

            elem = a.GetByName("1");
            Assert.AreEqual(new Tuple<int, double>(1, 0.7), elem);

            elem = a.GetByName("2");
            Assert.AreEqual(new Tuple<int, double>(2, 0.6), elem);

            elem = a.GetByName("3");
            Assert.AreEqual(new Tuple<int, double>(3, 0.8), elem);

            elem = a.GetByName("4");
            Assert.AreEqual(null, elem);
        }

        [TestMethod]
        public void BigSearchTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();

            for (var i = 0; i < 10e5; i++)
                a.TryAdd(i, i.ToString(), i * 0.0001);
            
            var expectedCount = 10e5;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

            var elem = a.GetByName("0");
            Assert.AreEqual(new Tuple<int, double>(0, 0), elem);

        }

        [TestMethod]
        public void ConcurrentAddTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();

            var threads = new Thread[10];
            for (var i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => a.TryAdd(i, i.ToString(), i));
                threads[i].Start();
            }
            foreach(var thread in threads)
                thread.Join();
            
            var expectedCount = 10;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

        }

        [TestMethod]
        public void ConcurrentRemoveTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();

            for (var i = 0; i < 15; i++)
                a.TryAdd(i, i.ToString(), i);

            var threads = new Thread[10];
            for (var i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => a.Remove(3));
                threads[i].Start();
            }
            foreach (var thread in threads)
                thread.Join();
            
            var expectedCount = 14;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

        }

        [TestMethod]
        public void ConcurrentClearTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();

            for (var i = 0; i < 3; i++)
                a.TryAdd(i, i.ToString(), i);

            var threads = new Thread[4];
            for (var i = 0; i < threads.Length; i++)
            {
                threads[i] = i % 2 == 1 ? new Thread(() => a.TryAdd(i, i.ToString(), i)) : new Thread(() => a.Clear());
                threads[i].Start();
            }
            foreach (var thread in threads)
                thread.Join();

            var expectedCount = 1;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

        }
    }
}
