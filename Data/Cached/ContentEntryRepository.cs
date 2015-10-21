using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Infuz.API.SQL;
using Infuz.Utilities;

using Ninject;

using Site.Data.API;
using Site.Data.API.CacheKey;
using Site.Data.API.Repository;

namespace Site.Data.Cached
{
    public class ContentEntryRepository : IContentEntryRepository
    {
        [Inject]
        public IContentEntryRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        #region IContentEntryRepository Members

        public bool Save(string key, string title, string meta, string partialLocation, string thumbnailEntry, IEnumerable<string> backgrounds)
        {
            var allCacheKey = new AllContentEntryKey();
            Cache.Clear(allCacheKey);

            return BackingStore.Save(key, title, meta, partialLocation, thumbnailEntry, backgrounds);
        }

        public bool Delete(IContentEntry contentEntry)
        {
            var allCacheKey = new AllContentEntryKey();
            Cache.Clear(allCacheKey);

            return BackingStore.Delete(contentEntry);
        }

        public IEnumerable<IContentEntry> Get()
        {
            var cacheKey = new AllContentEntryKey();
            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<IContentEntry>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.Get(initialLoad));
        }

        public IContentEntry Get(String key)
        {
            return BackingStore.Get(key);
        }

        #endregion
    }
}
