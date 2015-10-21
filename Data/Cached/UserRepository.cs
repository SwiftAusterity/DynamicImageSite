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
    public class UserRepository : IUserRepository
    {
        [Inject]
        public IUserRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        //NoContentPiece to get from the cache
        public bool Save(string source, string url)
        {
            var allCacheKey = new AllUsersKey();
            Cache.Clear(allCacheKey);
            
            return BackingStore.Save(source, url);
        }

        public bool Delete(IUser user)
        {
            var allCacheKey = new AllUsersKey();
            var idCacheKey = new UserKey(user.ID);

            Cache.Clear(allCacheKey);
            Cache.Clear(idCacheKey);

            return BackingStore.Delete(user);
        }

        public IUser Get(Guid userId)
        {
            var cacheKey = new UserKey(userId);
            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IUser>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.Get(userId));
        }

        public IEnumerable<IUser> Get()
        {
            var cacheKey = new AllUsersKey();
            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<IUser>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.Get(initialLoad));
        }
    }
}
