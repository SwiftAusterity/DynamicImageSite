using System;
using System.Collections.Generic;
using System.Threading;
using Infuz.Utilities;
using Ninject;
using Site.Data.API;
using Site.Data.API.CacheKey;
using Site.Data.API.Repository;
using System.Linq;

namespace Site.Data.Cached
{
    public class LastFMRepository : ILastFMRepository
    {
        [Inject]
        public ILastFMRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        private sealed class LazyTime<T> where T : class
        {
            public DateTime LastQueued { get; private set; }
            public T Result { get; private set; }

            public LazyTime(DateTime when, T what)
            {
                LastQueued = when;
                Result = what;
            }
        }

        public IEnumerable<ILastFMTrack> GetLatestTracks(string username)
        {
            var cacheKey = new LatestTrackKey(username);
            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<LazyTime<IEnumerable<ILastFMTrack>>>(keyString);

            // we do lazy, time-boxed access for this data...
            if (cachedResult == null
                || cachedResult.LastQueued < DateTime.UtcNow.AddMinutes(5))
            {
                // encache an initial value with a null result so we can looksie once in a while
                Cache.Enroll(cacheKey, (bool initialLoad) => new LazyTime<IEnumerable<ILastFMTrack>>(DateTime.UtcNow, Enumerable.Empty<ILastFMTrack>()));

                // now queue up a background refresh of the result to replace the bogus "don't know"
                // answer
                ThreadPool.QueueUserWorkItem(
                    (state) =>
                    {
                        var parms = state as Tuple<LatestTrackKey, string>;
                        Cache.Enroll(
                            parms.First
                            , (bool initialLoad) =>
                            {
                                var tracks = BackingStore.GetLatestTracks(parms.Second, initialLoad);
                                return new LazyTime<IEnumerable<ILastFMTrack>>(DateTime.UtcNow, tracks);
                            }
                        );
                    }
                    , TupleHelper.New(cacheKey, username));
            }

            return cachedResult == null ? null : cachedResult.Result;
        }

        public IEnumerable<ILastFMAlbum> GetLatestAlbums(string groupName)
        {
            var cacheKey = new LatestAlbumsKey(groupName);
            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<LazyTime<IEnumerable<ILastFMAlbum>>>(keyString);

            // we do lazy, time-boxed access for this data...
            if (cachedResult == null
                || cachedResult.LastQueued < DateTime.UtcNow.AddMinutes(5))
            {
                // encache an initial value with a null result so we can looksie once in a while
                Cache.Enroll(cacheKey, (bool initialLoad) => new LazyTime<IEnumerable<ILastFMAlbum>>(DateTime.UtcNow, Enumerable.Empty<ILastFMAlbum>()));

                // now queue up a background refresh of the result to replace the bogus "don't know"
                // answer
                ThreadPool.QueueUserWorkItem(
                    (state) =>
                    {
                        var parms = state as Tuple<LatestAlbumsKey, string>;
                        Cache.Enroll(
                            parms.First
                            , (bool initialLoad) =>
                            {
                                var albums = BackingStore.GetLatestAlbums(parms.Second, initialLoad);
                                return new LazyTime<IEnumerable<ILastFMAlbum>>(DateTime.UtcNow, albums);
                            }
                        );
                    }
                    , TupleHelper.New(cacheKey, groupName));
            }

            return cachedResult == null ? null : cachedResult.Result;
        }
    }
}
