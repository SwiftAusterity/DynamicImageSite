using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API.Repository
{
    public interface IContentEntryRepository
    {
        bool Save(string key, string title, string meta, string partialLocation, string thumbnailUrl, IEnumerable<string> backgrounds);
        bool Delete(IContentEntry contentEntry);
        IEnumerable<IContentEntry> Get();
        IContentEntry Get(string key);
    }

    public interface IContentEntryRepositoryBackingStore : IContentEntryRepository
    {
        IEnumerable<IContentEntry> Get(bool initial);
    }
}
