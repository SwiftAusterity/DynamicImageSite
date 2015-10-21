using System;

namespace Site.Data.API.CacheKey
{
    public class NewsFeedItemCacheKey : BaseCacheKey
    {
        public string Endpoint { get; set; }

        public NewsFeedItemCacheKey(String endpoint)
            : base("NewsFeedItemCache")
        {
            Endpoint = endpoint;
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
                return base.Key + "." + Endpoint;
            }
        }

        public override string PolicyKey
        {
            get
            {
                // lookup in the config the policy for the official key-an-parameters given
                string policyKey = base.PolicyKey;

                policyKey += "Endpoint/";

                return policyKey;
            }
        }
    }
}
