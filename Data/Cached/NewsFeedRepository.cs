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
    public class NewsFeedRepository : INewsFeedRepository
    {
        [Inject]
        public INewsFeedRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        #region INewsFeedRepository Members

        public IEnumerable<INewsFeedItem> GetLatestFeed(string endpoint)
        {
            var cacheKey = new NewsFeedItemCacheKey(endpoint);
            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<INewsFeedItem>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.GetLatestFeed(endpoint, initialLoad));
        }

        #endregion
    }
}
