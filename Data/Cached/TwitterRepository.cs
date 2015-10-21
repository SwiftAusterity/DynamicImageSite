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
    public class TwitterRepository : ITwitterRepository
    {
        [Inject]
        public ITwitterRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        #region ITwitterRepository Members

        public IEnumerable<ITweet> GetLatestTweet(string username)
        {
            var cacheKey = new TweetCacheKey(username);
            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<ITweet>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.GetLatestTweet(username, initialLoad));
        }

        #endregion
    }
}
