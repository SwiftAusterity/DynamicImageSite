using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Configuration;

using Site.Data.API;
using Site.Data.API.Repository;
using Site.Data.DTO;

using Infuz.Utilities;

using Newtonsoft.Json.Linq;

using Ninject;

using Site.Data.Ajax;
using LinqToTwitter;

namespace Site.Data.Live
{
    public class TwitterRepository : AjaxRepository, ITwitterRepositoryBackingStore
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public ITwitterRepository Me { get; set; }

        protected string UrlBase
        {
            get
            {
                return WebConfigurationManager.AppSettings["TwitterTarget"];
            }
        }

        #region ITwitterRepositoryBackingStore Members

        public IEnumerable<ITweet> GetLatestTweet(string username, bool initial)
        {
            var list = new List<ITweet>();

            try
            {
                using (var twitterCtx = new TwitterContext())
				{
                	twitterCtx.Timeout = 5;

	                var queryResults =
	                    from search in twitterCtx.Search
	                    where search.Type == SearchType.Search &&
	                          search.PersonFrom == username &&
	                          search.Page == 1 &&
	                          search.PageSize == 1 &&
	                          search.ResultType == ResultType.Recent 
	                    select search;

	                foreach (var search in queryResults)
	                    foreach (var entry in search.Entries)
	                    {
	                        var data = ReadFromAtomEntry(entry);

	                        if (data != null)
	                            list.Add(data);
	                    }
				}
            }
            catch //dont fail on social stuff
            {
            }

            return list;
        }

        #endregion

        #region ITwitterRepository Members

        public IEnumerable<ITweet> GetLatestTweet(string username)
        {
            return GetLatestTweet(username, true);
        }

        #endregion

        internal ITweet ReadFromAtomEntry(AtomEntry entry)
        {
            //dumb hack to stop @ messages from showing up.
            if (entry == null || string.IsNullOrEmpty(entry.Content) || entry.Content.StartsWith("@"))
                return null;

            var url = entry.Alternate;
            var body = entry.Content;
            var createdAt = entry.Published;
            var profileImageUrl = entry.Image;
            var userName = entry.Author.Name;

            var item = Kernel.Get<Tweet>();
            item.Url = url;
            item.ExpandedUrl = url;
            item.Body = body;
            item.CreatedAt = createdAt;
            item.DisplayUrl = url;
            item.ProfileImageUrl = profileImageUrl;
            item.UserName = userName;

            return item;
        }
    }
}