using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCollections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestMyCollections
{
    [TestClass]
    public class TestConcurrentDoubleKeyDictionary
    {
        [TestMethod]
        public void MyTest()
        {
            // Создаем конкурентный словарь 
            var cd = new ConcurrentDictionary<string, int>();
            var cdkd = new ConcurrentDoubleKeyDictionary<string, int, int>();
            

            // Хотим получить элемент с ключом "b", если нет - создаем  
            //int value = cd.GetOrAdd("b", (key) => 555);
            // Проверяем: value = 555; 
            //value = cd.GetOrAdd("b", -333);
            // Параллельно пытаемся обновить элемент с ключом "a" 
            Parallel.For(0, 100000, i =>
            {
                // Если ключа нет – добавляем 
                // Если есть – обновляем значение 
                cd.TryAdd(i.ToString(), i);
            });

            Parallel.For(0, 100000, i =>
             {
                 cdkd.TryAdd(i.ToString(), i, i);
             });

            Assert.AreEqual(100000, cd.Count);
            Assert.AreEqual(100000, cdkd.Count);
        }
    }
}
