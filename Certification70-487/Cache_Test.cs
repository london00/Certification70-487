using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Caching;
using System.Threading;

namespace Certification70_487
{
    [TestClass]
    public class Cache_Test
    {
        [DataRow("Cache1", 1)]
        [DataRow("Cache2", 2)]
        [DataRow("Cache3", 3)]
        [TestMethod]
        public void CanCache(string key, int value)
        {
            // Arrange
            ObjectCache cache = MemoryCache.Default;

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = new System.DateTimeOffset(DateTime.Now.AddMinutes(1))
            };

            // ACT
            cache.Remove(key);
            cache.Add(key, value, policy);
            int fetchedValue = (int)cache.Get(key);


            // Assert
            Assert.AreEqual(fetchedValue, value, "Cached value is not the expected one.");
        }

        [DataRow("Cache1", 1)]
        [DataRow("Cache2", 2)]
        [DataRow("Cache3", 3)]
        [TestMethod]
        public void TestSlidingExpiration(string key, int value)
        {
            // Arrange
            ObjectCache cache = MemoryCache.Default;

            var policy = new CacheItemPolicy
            {
                SlidingExpiration = TimeSpan.FromSeconds(1)
            };

            // ACT
            cache.Set(key, value, policy);
            int fetchedValue = 0;

            for (int i = 0; i < 10; i++)
            {
                fetchedValue = (int)cache.Get(key);
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
                Assert.AreEqual(fetchedValue, value, "Cache has expired");
            }

            Thread.Sleep(TimeSpan.FromSeconds(2));
            fetchedValue = int.Parse(cache.Get("key")?.ToString() ?? "0");
            Assert.AreNotEqual(fetchedValue, value, "Cache not expired");
        }
    }
}
