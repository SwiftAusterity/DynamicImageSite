using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Caching;

using Infuz.Utilities;

using Site.Data.API;
using Site.Data.API.CacheKey;

namespace Site.Data.Cached
{
    public class HttpCacheShim : ICache
    {
        public T Get<T>(string key)
        {
            return (T)HttpRuntime.Cache.Get(key);
        }

        public T Enroll<T>(BaseCacheKey key, Func<bool, T> filler)
        {
            var parameters = BuildParameters<T>(key.Policy, filler);
            var value = parameters.Fill(key.Key, true);
            InternalPut(key.Key, value, parameters);
            return value;
        }

        public IList<TElement> GetCollection<TCollectionKey, TElement, TElementKey>(TCollectionKey collectionKey)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey
        {
            var keys = Get<IList<TElementKey>>(collectionKey.Key);

            if (keys == null)
                return null;

            var elements = new List<TElement>(keys.Count);

            foreach (var key in keys)
            {
                var element = Get<TElement>(key.Key);

                if (element == null)
                    return null;

                elements.Add(element);
            }

            return elements;
        }

        public IList<TElement> EnrollCollection<TCollectionKey, TElement, TElementKey>(
                TCollectionKey key,
                Func<bool, IList<TElement>> filler,
                Func<TCollectionKey, TElement, TElementKey> keyProjection)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey
        {
            var parameters = BuildParameters(key.Policy,
               (bool initialFill) =>
               {
                   var curriedBaseKey = key;
                   var curriedFiller = filler;
                   var curriedProjector = keyProjection;
                   return Refiller(TupleHelper.New(curriedBaseKey, curriedFiller, curriedProjector));
               });

            var cachedKeys = parameters.Fill(key.Key, true);
            InternalPut(key.Key, cachedKeys, parameters);

            var collection = new List<TElement>(cachedKeys.Count);
            foreach (var cachedKey in cachedKeys)
            {
                var element = this.Get<TElement>(cachedKey.Key);

                if (element == null)
                    return null;   // failed to find one... declare failure and go to backing store

                collection.Add(element);
            }

            return collection;
        }


        private IList<TElementKey> Refiller<TCollectionKey, TElement, TElementKey>(
            Tuple<TCollectionKey, Func<bool, IList<TElement>>, Func<TCollectionKey, TElement, TElementKey>> args)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey
        {
            var key = args.First;
            var filler = args.Second;
            var keyProjection = args.Third;

            var list = Enroll(key, filler);

            if (list == null)
                return null;

            var keys = new List<TElementKey>(list.Count);

            foreach (var element in list)
            {
                var elementKey = keyProjection(key, element);
                InternalPut(elementKey.Key,
                             element,
                             BuildParameters(elementKey.Policy,
                                             (bool initialFiller) =>
                                             {
                                                 // this ensures we can yield the value
                                                 // even if the cache flushes
                                                 var curriedElement = element;
                                                 return curriedElement;
                                             }));
                keys.Add(elementKey);
            }

            return keys;
        }

        protected virtual CacheAddParameters<T> BuildParameters<T>(CachePolicy policy, Func<bool, T> filler)
        {
            return new CacheAddParameters<T>(policy, filler);
        }

        private void InternalPut<T>(string key, T value, CacheAddParameters<T> parameters)
        {
            if (value == null)
            {
                Debug.WriteLine(key + " {REMOVE}", "InternalPut");
                HttpRuntime.Cache.Remove(key);
            }
            else
            {
                CacheItemUpdateCallback callback = MakeCallback(parameters);
                var absoluteExpiration = parameters.AbsoluteExpiration;
                var slidingTimeout = parameters.SlidingTimeout;
                Debug.WriteLine(key, "InternalPut");
                HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, slidingTimeout, callback);
            }
        }

        private CacheItemUpdateCallback MakeCallback<T>(CacheAddParameters<T> parameters)
        {
            CacheItemUpdateCallback callback = (string updatedKey, CacheItemUpdateReason reason, out object expensiveObject, out CacheDependency dependency, out DateTime absoluteExpiration, out TimeSpan slidingExpiration) =>
            {
                HandleExpiration<T>(this, parameters, updatedKey, reason, out expensiveObject, out dependency, out absoluteExpiration, out slidingExpiration);
            };

            return callback;
        }

        protected class CacheAddParameters<T>
        {
            public TimeSpan SlidingTimeout { get; private set; }
            public int NumberOfRefillsRemaining { get; set; }
            public Func<bool, T> Filler { get; private set; }

            int _absoluteSeconds;
            public DateTime AbsoluteExpiration
            {
                get
                {
                    if (_absoluteSeconds == CachePolicy.Unused)
                        return Cache.NoAbsoluteExpiration;

                    var absolute = DateTime.UtcNow.AddSeconds(_absoluteSeconds);
                    return absolute;
                }
            }

            public bool ShouldScheduleRefresh
            {
                get
                {
                    // Check if infinite refills are allowed
                    if (NumberOfRefillsRemaining == CachePolicy.Infinite)
                        return true;

                    // if no more refills are allowed (started at <= 0), or used them up
                    if (NumberOfRefillsRemaining <= 0)
                        return false;

                    return true;
                }
            }

            public CacheAddParameters(CachePolicy policy, Func<bool, T> filler)
            {
                _absoluteSeconds = policy.AbsoluteSeconds;

                Filler = filler;
                SlidingTimeout = policy.SlidingSeconds == CachePolicy.Unused
                    ? Cache.NoSlidingExpiration
                    : TimeSpan.FromSeconds(policy.SlidingSeconds);
                NumberOfRefillsRemaining = policy.RefillCount;
            }

            public T Fill(string key, bool initialFill)
            {
                if (initialFill)
                    Debug.WriteLine(key, "InitialFill");
                else
                {
                    if (NumberOfRefillsRemaining > 0)
                        NumberOfRefillsRemaining--;

                    Debug.WriteLine(key + " remaining " + NumberOfRefillsRemaining.ToString(), "CacheRefill");
                }

                T result = Filler(initialFill);
                EmitResultDebug(key, result);
                return result;
            }

            [Conditional("DEBUG")]
            void EmitResultDebug(string key, T result)
            {
                var resultString = result == null ? "{null}" : result.ToString();
                Debug.WriteLine(key + " " + resultString, "Fill returned");
            }
        }

        private static void HandleExpiration<T>(HttpCacheShim that
            , CacheAddParameters<T> parameters
            , string cacheKey
            , CacheItemUpdateReason reason
            , out object expensiveObject
            , out CacheDependency dependency
            , out DateTime absoluteExpiration
            , out TimeSpan slidingExpiration)
        {
            Debug.WriteLine(cacheKey, "CacheExpired");

            expensiveObject = null;
            dependency = null;
            absoluteExpiration = Cache.NoAbsoluteExpiration;
            slidingExpiration = Cache.NoSlidingExpiration;

            // if we were not shutting down, might want to handle the reuse/refresh
            if (reason == CacheItemUpdateReason.Expired
                && !AppDomain.CurrentDomain.IsFinalizingForUnload())
            {
                if (parameters.ShouldScheduleRefresh)
                {
                    // we need queue a request to the underlying store to get more current data into the cache so it stays primed.
                    Debug.WriteLine(cacheKey, "Queueing refill");
                    expensiveObject = parameters.Fill(cacheKey, false);
                    absoluteExpiration = parameters.AbsoluteExpiration;
                    slidingExpiration = parameters.SlidingTimeout;
                }
            }
        }

        public void Clear()
        {
        }

        public void Clear(BaseCacheKey key)
        {
            HttpRuntime.Cache.Remove(key.Key);
        }
    }
}
