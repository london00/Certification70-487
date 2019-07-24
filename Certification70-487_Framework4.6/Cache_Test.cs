using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Caching;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Certification70_487_Framework4._6
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

        [DataRow("testfile", "test.txt")]
        [TestMethod]
        public void FileChangeMonitor(string key, string filePath)
        {
            string textFromFile = null;
            filePath = Path.Combine(AppContext.BaseDirectory, filePath);

            var cacheInstance = MemoryCache.Default;

            var policy = new CacheItemPolicy
            {
                Priority = CacheItemPriority.Default,
                AbsoluteExpiration = DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
            };

            policy.ChangeMonitors.Add(new HostFileChangeMonitor(new[] { filePath }));

            textFromFile = File.ReadAllText(filePath);
            cacheInstance.Set(key, textFromFile, policy);

            var endsAt = DateTime.UtcNow.AddMinutes(5);

            while (DateTime.UtcNow < endsAt)
            {
                string textFromCache = cacheInstance.Get(key)?.ToString() ?? null;
                if (textFromCache == null)
                {
                    textFromFile = File.ReadAllText(filePath);

                    Debug.WriteLine("file has changed: ");
                    Debug.WriteLine(textFromFile);

                    var newPolicy = new CacheItemPolicy
                    {
                        Priority = CacheItemPriority.Default,
                        AbsoluteExpiration = DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
                    };

                    newPolicy.ChangeMonitors.Add(new HostFileChangeMonitor(new[] { filePath }));
                    cacheInstance.Set(key, textFromFile, newPolicy);
                }
                else
                {
                    Debug.WriteLine("From cache: ");
                    Debug.WriteLine(textFromCache);
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}
