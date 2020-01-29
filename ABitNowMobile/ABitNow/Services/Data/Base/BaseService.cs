using ABitNow.Models;
using Akavache;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ABitNow.Services.Data.Base
{
    public class BaseService
    {
        protected IBlobCache Cache;

        public BaseService(IBlobCache cache)
        {
            Cache = cache ?? BlobCache.LocalMachine;
        }

        public async Task<T> GetFromCache<T>(string cacheName)
        {
            try
            {
                T t = await Cache.GetObject<T>(cacheName);
                return t;
            }
            catch (KeyNotFoundException)
            {
                return default(T);
            }
        }

        public void InvalidateAllCache()
        {
            Cache.InvalidateAll();
        }

        public void InvalidateCacheByKey(string key)
        {
            Cache.Invalidate(key);
        }
    }
}
