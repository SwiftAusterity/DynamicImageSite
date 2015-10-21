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
    public class FacebookRepository : IFacebookRepository
    {
        [Inject]
        public IFacebookRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        #region IFacebookRepository Members

        public IEnumerable<IFacebookPost> GetLatestFeed(string source, string accessToken)
        {
            var cacheKey = new FacebookCacheKey(source);
            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<IFacebookPost>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.GetLatestFeed(source, accessToken, initialLoad));
        }

        #endregion
    }
}
