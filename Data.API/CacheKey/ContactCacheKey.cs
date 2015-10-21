using System;

namespace Site.Data.API.CacheKey
{
    public class ContactByEmailKey : BaseCacheKey
    {
        public string Email { get; set; }

        public ContactByEmailKey(string email)
            : base("ContactByEmail")
        {
            Email = email;
        }

        public override string Key
        {
            get
            {
                return string.Format("{0}.{1}", base.Key, Email);
            }
        }

        public override string PolicyKey
        {
            get
            {
                // lookup in the config the policy for the official key-an-parameters given
                return string.Format("{0}{1}/", base.PolicyKey, "Email");
            }
        }
    }

    public class AllContactKey : BaseCacheKey
    {
        public AllContactKey()
            : base("AllContact")
        {
        }
    }
}
