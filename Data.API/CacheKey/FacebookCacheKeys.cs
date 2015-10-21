using System;

namespace Site.Data.API.CacheKey
{
    public class FacebookCacheKey : BaseCacheKey
    {
        public string Source { get; set; }

        public FacebookCacheKey(String source)
            : base("FacebookCache")
        {
            Source = source;
        }

        public override CachePolicy DefaultPolicy
        {
            get
            {
                int refillCount = CachePolicy.Infinite;
                int absoluteSeconds = 300;  // we can cache for at-most 5 mins

                var policy = new CachePolicy
                {
                    AbsoluteSeconds = absoluteSeconds,
                    RefillCount = refillCount
                };
                return policy;
            }
        }
        public override string Key
        {
            get
            {
                return base.Key + "." + Source;
            }
        }

        public override string PolicyKey
        {
            get
            {
                // lookup in the config the policy for the official key-an-parameters given
                string policyKey = base.PolicyKey;

                policyKey += "Source/";

                return policyKey;
            }
        }
    }
}
