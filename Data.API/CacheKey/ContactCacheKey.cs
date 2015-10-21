using System;

namespace Site.Data.API.CacheKey
{
    public class ContactByEmailKey : BaseCacheKey
    {
        public String Email { get; set; }

        public ContactByEmailKey(String email)
            : base("ContactByEmail")
        {
            Email = email;
        }

        public override string Key
        {
            get
            {
                return String.Format("{0}.{1}", base.Key, Email);
            }
        }

        public override string PolicyKey
        {
            get
            {
                // lookup in the config the policy for the official key-an-parameters given
                return String.Format("{0}{1}/", base.PolicyKey, "Email");
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
