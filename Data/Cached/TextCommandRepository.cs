using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Infuz.API.SQL;
using Infuz.Utilities;

using Ninject;

using Site.Data.API;
using Site.Data.API.CacheKey;
using Site.Data.API.Repository;

namespace Site.Data.Cached
{
    public class TextCommandRepository : ITextCommandRepository
    {
        [Inject]
        public ITextCommandRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        #region IContactRepository Members

        public bool Save(string command, string path, bool handled)
        {
            return BackingStore.Save(command, path, handled);
        }

        #endregion
    }
}
