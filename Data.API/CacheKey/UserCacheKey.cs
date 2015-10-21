using System;

namespace Site.Data.API.CacheKey
{
    public class UserKey : BaseCacheKey
    {
        public Guid ID { get; set; }

        public UserKey(Guid userId)
            : base("User")
        {
            ID = userId;
        }

        public override string Key
        {
            get
            {
                return string.Format("{0}.{1}", base.Key, ID);
            }
        }

        public override string PolicyKey
        {
            get
            {
                // lookup in the config the policy for the official key-an-parameters given
                return string.Format("{0}{1}/", base.PolicyKey, "ID");
            }
        }
    }

    public class AllUsersKey : BaseCacheKey
    {
        public AllUsersKey()
            : base("AllUsers")
        {
        }
    }
}
