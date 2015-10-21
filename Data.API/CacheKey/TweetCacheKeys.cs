using System;

namespace Site.Data.API.CacheKey
{
    public class TweetCacheKey : BaseCacheKey
    {
        public string Username { get; set; }

        public TweetCacheKey(String userName)
            : base("TweetCache")
        {
            Username = userName;
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
                return base.Key + "." + Username;
            }
        }

        public override string PolicyKey
        {
            get
            {
                // lookup in the config the policy for the official key-an-parameters given
                string policyKey = base.PolicyKey;

                policyKey += "Username/";

                return policyKey;
            }
        }
    }
}
