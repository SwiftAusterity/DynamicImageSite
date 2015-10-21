using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API.Repository
{
    public interface ITwitterRepository
    {
        IEnumerable<ITweet> GetLatestTweet(String username);
    }

    public interface ITwitterRepositoryBackingStore : ITwitterRepository
    {
        IEnumerable<ITweet> GetLatestTweet(String username, bool initial);
    }
}
