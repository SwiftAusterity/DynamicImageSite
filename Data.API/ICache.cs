using System;
using System.Collections.Generic;
using Site.Data.API.CacheKey;

namespace Site.Data.API
{
    public interface ICache
    {
        T Get<T>(string key);
        T Enroll<T>(BaseCacheKey key, Func<bool, T> filler);

        IList<TElement> GetCollection<TCollectionKey, TElement, TElementKey>(TCollectionKey collectionKey)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey;
        IList<TElement> EnrollCollection<TCollectionKey, TElement, TElementKey>(
                TCollectionKey key,
                Func<bool, IList<TElement>> filler,
                Func<TCollectionKey, TElement, TElementKey> keyProjection)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey;

        void Clear();
        void Clear(BaseCacheKey key);
    }
}
