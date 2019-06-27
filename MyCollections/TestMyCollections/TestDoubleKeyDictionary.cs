using System;
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
            a.Add(new Tuple<int, string, double>(0, "0", 0.5));
            Assert.AreEqual(1, a.Count);
        }

        [TestMethod]
        public void GetIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(new Tuple<int, string, double>(0, "0", 0.5));
            var expected = new Tuple<int, double>(0, 0.5);
            var actual = a.GetById(0);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetNameTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(new Tuple<int, string, double>(0, "0", 0.5));
            var expected = new Tuple<string, double>("0", 0.5);
            var actual = a.GetByName("0");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(new Tuple<int, string, double>(0, "0", 0.5));
            a.Clear();
            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveOneByIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(new Tuple<int, string, double>(0, "0", 0.5));

            a.Remove(0);

            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveOneByNameTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(new Tuple<int, string, double>(0, "0", 0.5));

            a.Remove("0");

            var expected = 0;
            var actual = a.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveByIdTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(new Tuple<int, string, double>(0, "0", 0.5));
            a.Add(new Tuple<int, string, double>(2, "2", 0.7));
            a.Add(new Tuple<int, string, double>(1, "1", 0.6));
            a.Add(new Tuple<int, string, double>(3, "3", 0.8));
            a.Add(new Tuple<int, string, double>(4, "4", 0.9));

            a.Remove(1);

            var expectedCount = 4;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

            var elem = a.GetById(0);
            Assert.AreEqual(new Tuple<int, double>(0, 0.5), elem);

            elem = a.GetById(1);
            Assert.AreEqual(null, elem);

            elem = a.GetById(2);
            Assert.AreEqual(new Tuple<int, double>(2, 0.7), elem);

            elem = a.GetById(3);
            Assert.AreEqual(new Tuple<int, double>(3, 0.8), elem);

            elem = a.GetById(4);
            Assert.AreEqual(new Tuple<int, double>(4, 0.9), elem);
        }

        [TestMethod]
        public void RemoveByNameTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(new Tuple<int, string, double>(0, "0", 0.5));
            a.Add(new Tuple<int, string, double>(2, "2", 0.6));
            a.Add(new Tuple<int, string, double>(1, "1", 0.7));
            a.Add(new Tuple<int, string, double>(3, "3", 0.8));
            a.Add(new Tuple<int, string, double>(4, "4", 0.9));

            a.Remove("1");

            var expectedCount = 4;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

            var elem = a.GetById(0);
            Assert.AreEqual(new Tuple<int, double>(0, 0.5), elem);

            elem = a.GetById(1);
            Assert.AreEqual(null, elem);

            elem = a.GetById(2);
            Assert.AreEqual(new Tuple<int, double>(2, 0.6), elem);

            elem = a.GetById(3);
            Assert.AreEqual(new Tuple<int, double>(3, 0.8), elem);

            elem = a.GetById(4);
            Assert.AreEqual(new Tuple<int, double>(4, 0.9), elem);
        }

        [TestMethod]
        public void RemoveLastTest()
        {
            var a = new DoubleKeyDictionary<int, string, double>();
            a.Add(new Tuple<int, string, double>(0, "0", 0.5));
            a.Add(new Tuple<int, string, double>(1, "1", 0.7));
            a.Add(new Tuple<int, string, double>(2, "2", 0.6));
            a.Add(new Tuple<int, string, double>(3, "3", 0.8));
            a.Add(new Tuple<int, string, double>(4, "4", 0.9));

            a.Remove("4");

            var expectedCount = 4;
            var actualCount = a.Count;
            Assert.AreEqual(expectedCount, actualCount);

            var elem = a.GetById(0);
            Assert.AreEqual(new Tuple<int, double>(0, 0.5), elem);

            elem = a.GetById(1);
            Assert.AreEqual(new Tuple<int, double>(1, 0.7), elem);

            elem = a.GetById(2);
            Assert.AreEqual(new Tuple<int, double>(2, 0.6), elem);

            elem = a.GetById(3);
            Assert.AreEqual(new Tuple<int, double>(3, 0.8), elem);

            elem = a.GetById(4);
            Assert.AreEqual(null, elem);
        }
    }
}
