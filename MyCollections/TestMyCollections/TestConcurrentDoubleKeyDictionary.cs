using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCollections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var xxxx = new DoubleKeyDictionary<int, int, int>();

            xxxx.TryAdd(0, 0, 0);

            // Хотим получить элемент с ключом "b", если нет - создаем  
            int value = cd.GetOrAdd("b", (key) => 555);
            // Проверяем: value = 555; 
            value = cd.GetOrAdd("b", -333);
            var xval = xxxx[0, 0];
            // Параллельно пытаемся обновить элемент с ключом "a" 
            Parallel.For(0, 100000, i =>
            {
                // Если ключа нет – добавляем 
                // Если есть – обновляем значение 
                cd.AddOrUpdate("a", 1, (key, oldValue) => oldValue + 1);
            });

            Parallel.For(0, 100000, i =>
            {
                // Если ключа нет – добавляем 
                // Если есть – обновляем значение 
                xxxx.TryAdd(i, i, i);
            });

            var a = cd["a"];
            var b = cd["b"];
        }
    }
}
