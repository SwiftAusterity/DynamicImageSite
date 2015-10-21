using System;

namespace Site.Data.API.CacheKey
{
    public interface IBaseCacheKey
    {
        string Prefix { get; set; }
        string SubKey { get; set; }
        string Key { get; }
        string PolicyKey { get; }
        CachePolicy DefaultPolicy { get; }
        CachePolicy Policy { get; }
    }
    public abstract class BaseCacheKey : IBaseCacheKey
    {
        public string Prefix { get; set; }
        public string SubKey { get; set; }

        public BaseCacheKey(string prefix)
        {
            Prefix = prefix;
        }

        public virtual string Key
        {
            get
            {
                return String.IsNullOrEmpty(SubKey)
                            ?  Prefix 
                            : Prefix + "." + SubKey;
            }
        }

        public virtual string PolicyKey
        {
            get
            {
                return (String.IsNullOrEmpty(SubKey)
                            ? Prefix 
                            : Prefix + "/" + SubKey) + "/";
            }
        }

        public virtual CachePolicy DefaultPolicy
        {
            get
            {
                var policy =  new CachePolicy();
                policy.AbsoluteSeconds = 10;    // every ten second should pepper the server nicely
                return policy;
            }
        }

        public virtual CachePolicy Policy
        {
            get
            {
                // lookup in the config the policy for the official key-an-parameters given
                var policyKey = PolicyKey;

                // lookup the policy in the web.config

                // if we didn't find one, defer to the default policy
                return DefaultPolicy;
            }
        }
    }
}
