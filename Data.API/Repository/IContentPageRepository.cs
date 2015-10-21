using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API.Repository
{
    public interface IContentPageRepository
    {
        bool Save(String url, String title, String meta, String partialLocation
            , IEnumerable<String> backgrounds, IEnumerable<INavElement> sectionNav, IEnumerable<INavElement> subNav
            , INavElement forwardNav, INavElement backwardNav);
        bool Delete(IContentPage contentPiece);
        IEnumerable<IContentPage> Get(bool mobile);
        IContentPage Get(String url, bool mobile);
    }

    public interface IContentPageRepositoryBackingStore : IContentPageRepository
    {
        IEnumerable<IContentPage> Get(bool mobile, bool initial);
    }
}
