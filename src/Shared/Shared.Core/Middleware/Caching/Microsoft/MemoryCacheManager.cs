using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Utilies.IoC;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Shared.Core.Middleware.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        #region Variable

        private readonly IMemoryCache _cache;

        #endregion

        #region Constructor

        public MemoryCacheManager()
            : this(ServiceTool.ServiceProvider.GetService<IMemoryCache>())
        {
        }

        public MemoryCacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        #endregion

        #region Methods

        #region Public Methods

        public void Add(string key, object data, int duration)
            => _cache.Set(key, data, TimeSpan.FromMinutes(duration));

        public void Add(string key, object data)
            => _cache.Set(key, data);

        public T Get<T>(string key)
            => _cache.Get<T>(key);

        public object Get(string key)
            => _cache.Get(key);

        public bool IsAdd(string key)
            => _cache.TryGetValue(key, out _);

        public void Remove(string key)
            => _cache.Remove(key);

        public void RemoveByPattern(string pattern)
        {
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty(
                "EntriesCollection",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_cache) as dynamic;

            var cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key)
                .ToList();
            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }
        }

        #endregion

        #endregion

    }
}