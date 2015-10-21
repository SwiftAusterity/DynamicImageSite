using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API.Repository
{
    public interface INewsFeedRepository
    {
        IEnumerable<INewsFeedItem> GetLatestFeed(String endpoint);
    }

    public interface INewsFeedRepositoryBackingStore : INewsFeedRepository
    {
        IEnumerable<INewsFeedItem> GetLatestFeed(String endpoint, bool initial);
    }
}
