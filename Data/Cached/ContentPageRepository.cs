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
    public class ContentPageRepository : IContentPageRepository
    {
        [Inject]
        public IContentPageRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        #region IContentPageRepository Members

        public bool Save(string url, string title, string meta, string partialLocation
            , IEnumerable<string> backgrounds, IEnumerable<INavElement> sectionNav, IEnumerable<INavElement> subNav
            , INavElement forwardNav, INavElement backwardNav)
        {
            var allMobileCacheKey = new AllContentPageKey(true);
            var allCacheKey = new AllContentPageKey(false);
            Cache.Clear(allCacheKey);
            Cache.Clear(allMobileCacheKey);

            return BackingStore.Save(url, title, meta, partialLocation, backgrounds, sectionNav, subNav, forwardNav, backwardNav);
        }

        public bool Delete(IContentPage contentPiece)
        {
            var allMobileCacheKey = new AllContentPageKey(true);
            var allCacheKey = new AllContentPageKey(false);
            Cache.Clear(allCacheKey);
            Cache.Clear(allMobileCacheKey);

            return BackingStore.Delete(contentPiece);
        }

        public IEnumerable<IContentPage> Get(bool mobile)
        {
            var cacheKey = new AllContentPageKey(mobile);
            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<IContentPage>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.Get(mobile, initialLoad));
        }

        public IContentPage Get(String url, bool mobile)
        {
            return BackingStore.Get(url, mobile);
        }

        #endregion
    }
}
