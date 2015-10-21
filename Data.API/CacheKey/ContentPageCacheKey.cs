using System;

namespace Site.Data.API.CacheKey
{
    public class ContentPageKey : BaseCacheKey
    {
        public string URL { get; set; }
        public bool Mobile { get; set; }

        public ContentPageKey(string url, bool mobile)
            : base("ContentPage")
        {
            URL = url;
            Mobile = mobile;
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
                return base.Key + "." + URL + "." + Mobile;
            }
        }

        public override string PolicyKey
        {
            get
            {
                // lookup in the config the policy for the official key-an-parameters given
                string policyKey = base.PolicyKey;

                policyKey += "URL/";
                policyKey += "Mobile/";

                return policyKey;
            }
        }
    }

    public class AllContentPageKey : BaseCacheKey
    {
        public bool Mobile { get; set; }

        public AllContentPageKey(bool mobile)
            : base("AllContentPage")
        {
            Mobile = mobile;
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
                return base.Key + "." + Mobile;
            }
        }

        public override string PolicyKey
        {
            get
            {
                // lookup in the config the policy for the official key-an-parameters given
                string policyKey = base.PolicyKey;

                policyKey += "Mobile/";

                return policyKey;
            }
        }    }

    public class ContentEntryKey : BaseCacheKey
    {
        public string StringKey { get; set; }

        public ContentEntryKey(string key)
            : base("ContentEntry")
        {
            StringKey = key;
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
                return base.Key + "." + StringKey;
            }
        }

        public override string PolicyKey
        {
            get
            {
                // lookup in the config the policy for the official key-an-parameters given
                string policyKey = base.PolicyKey;

                policyKey += "StringKey/";

                return policyKey;
            }
        }
    }

    public class AllContentEntryKey : BaseCacheKey
    {
        public AllContentEntryKey()
            : base("AllContentEntry")
        {
        }
    }
}
