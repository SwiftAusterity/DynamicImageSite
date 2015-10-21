using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API.Repository
{
    public interface IContentEntryRepository
    {
        bool Save(String key, String title, String meta, String partialLocation, String thumbnailUrl, IEnumerable<String> backgrounds);
        bool Delete(IContentEntry contentEntry);
        IEnumerable<IContentEntry> Get();
        IContentEntry Get(String key);
    }

    public interface IContentEntryRepositoryBackingStore : IContentEntryRepository
    {
        IEnumerable<IContentEntry> Get(bool initial);
    }
}
