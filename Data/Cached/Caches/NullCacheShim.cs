using System;
using System.Collections.Generic;
using Site.Data.API;
using Site.Data.API.CacheKey;

namespace Site.Data.Cached
{
    public class NullCacheShim : ICache
    {
        public T Get<T>(string key)
        {
            return default(T);
        }

        public T Enroll<T>(BaseCacheKey key, Func<bool, T> filler)
        {
            T value = filler(true);
            return value;
        }

        public void Clear()
        {
        }

        public void Clear(BaseCacheKey key)
        {
        }

        public IList<TElement> GetCollection<TCollectionKey, TElement, TElementKey>(TCollectionKey collectionKey)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey
        {
            return null;
        }

        public IList<TElement> EnrollCollection<TCollectionKey, TElement, TElementKey>(
                TCollectionKey key,
                Func<bool, IList<TElement>> filler,
                Func<TCollectionKey, TElement, TElementKey> keyProjection)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey
        {
            var list = filler(true);

            if (list != null)
            {
                var keys = new List<TElementKey>(list.Count);

                foreach (var element in list)
                {
                    var elementKey = keyProjection(key, element);
                    keys.Add(elementKey);
                }
            }

            return list;
        }
    }
}
