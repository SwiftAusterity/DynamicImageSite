using System;

namespace Site.Data.API.CacheKey
{
    public class LatestTrackKey : BaseCacheKey
    {
        public string Username { get; set; }

        public LatestTrackKey(String userName)
            : base("LatestTrackKey")
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

    public class LatestAlbumsKey : BaseCacheKey
    {
        public string Groupname { get; set; }

        public LatestAlbumsKey(String groupName)
            : base("LatestAlbums")
        {
            Groupname = groupName;
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
                return base.Key + "." + Groupname;
            }
        }

        public override string PolicyKey
        {
            get
            {
                // lookup in the config the policy for the official key-an-parameters given
                string policyKey = base.PolicyKey;

                policyKey += "Groupname/";

                return policyKey;
            }
        }
    }
}
