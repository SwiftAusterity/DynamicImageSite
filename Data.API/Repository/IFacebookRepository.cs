using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API.Repository
{
    public interface IFacebookRepository
    {
        IEnumerable<IFacebookPost> GetLatestFeed(string endpoint, string accessToken);
    }

    public interface IFacebookRepositoryBackingStore : IFacebookRepository
    {
        IEnumerable<IFacebookPost> GetLatestFeed(string endpoint, string accessToken, bool initial);
    }
}
