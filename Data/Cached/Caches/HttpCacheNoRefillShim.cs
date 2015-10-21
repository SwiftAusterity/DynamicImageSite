using System;
using Site.Data.API;

namespace Site.Data.Cached
{
    public class HttpCacheNoRefillShim : HttpCacheShim
    {
        protected override CacheAddParameters<T> BuildParameters<T>(CachePolicy policy, Func<bool, T> filler)
        {
            var parameters = base.BuildParameters<T>(policy, filler);
            parameters.NumberOfRefillsRemaining = 0;
            return parameters;
        }
    }
}
