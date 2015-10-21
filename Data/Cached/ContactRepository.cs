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
    public class ContactRepository : IContactRepository
    {
        [Inject]
        public IContactRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        #region IContactRepository Members

        public bool Save(string email, string body, string name, bool subscribed)
        {
            var allCacheKey = new AllContactKey();
            Cache.Clear(allCacheKey);

            return BackingStore.Save(email, body, name, subscribed);
        }

        public bool Delete(IContact contact)
        {
            var allCacheKey = new AllContactKey();

            Cache.Clear(allCacheKey);

            return BackingStore.Delete(contact);
        }

        public IEnumerable<IContact> Get()
        {
            var cacheKey = new AllContactKey();
            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<IContact>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.Get(initialLoad));
        }

        public IEnumerable<IContact> Get(String emailAddress)
        {
            var cacheKey = new ContactByEmailKey(emailAddress);
            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<IContact>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.Get(emailAddress));
        }

        #endregion
    }
}
