using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Linq;

using Infuz.API.SQL;
using Infuz.Utilities;

using Ninject;

using Site.Data.API;
using Site.Data.API.Repository;
using Site.Data.File;
using Site.Data.DTO;
using System.Web.Configuration;

namespace Site.Data.Live
{
    public class ContentEntryRepository : FileRepository, IContentEntryRepositoryBackingStore
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public IContentEntryRepository Me { get; set; }

        protected string ContentLocation
        {
            get
            {
                return WebConfigurationManager.AppSettings["ContentFile"];
            }
        }

        #region IContentEntryRepositoryBackingStore Members

        public IEnumerable<IContentEntry> Get(bool initial)
        {
            return UntilDovesCry<IContentEntry>(ContentLocation
                                        , "ContentPages"
                                        , "ContentPage"
                                        , (node, results) => AppendFromDataReader(node, results));
        }

        #endregion

        #region IContentEntryRepository Members

        public bool Save(string key, string title, string meta, string partialLocation, string thumbnailUrl, IEnumerable<string> backgrounds)
        {
            throw new NotImplementedException();
        }

        public bool Delete(IContentEntry contentPiece)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IContentEntry> Get()
        {
            return Get(true);
        }

        public IContentEntry Get(string key)
        {
            var list = Me.Get();
            return list.FirstOrDefault(page => page.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion

        private void AppendFromDataReader(XmlNode node, IList<IContentEntry> result)
        {
            var data = ReadPageFrom(node);
            result.Add(data);
        }

        internal IContentEntry ReadPageFrom(XmlNode node)
        {
            if (node == null)
                return null;

            var backgrounds = GetBackgrounds(node.SelectNodes("Background"));
            var meta = node.SelectSingleNode("Meta").InnerText;

            var infoTextNode = node.SelectSingleNode("InfoText");

            var infoText = String.Empty;
            if (infoTextNode != null)
                infoText = infoTextNode.InnerText;

            var partialLocation = node.SelectSingleNode("PartialLocation").InnerText;
            var thumbnailUrl = node.SelectSingleNode("Thumbnail").InnerText;

            var title = node.SelectSingleNode("Title").InnerText;
            var key = node.GetAttribute<String>("key", String.Empty);

            var socialMediaNodes = node.SelectNodes("SocialFeed");

            var data = Kernel.Get<ContentEntry>();
            data.Backgrounds = backgrounds;
            data.Meta = meta;
            data.PartialLocation = partialLocation;
            data.ThumbnailUrl = thumbnailUrl;
            data.Title = title;
            data.Key = key;
            data.InfoText = infoText;

            if (socialMediaNodes != null)
                data.SocialMediaFeedItems = GetSocialFeed(socialMediaNodes);

            return data;
        }

        internal IEnumerable<String> GetBackgrounds(XmlNodeList nodes)
        {
            var returnList = new List<String>();

            foreach (XmlNode node in nodes)
                returnList.Add(node.InnerText);

            return returnList;
        }

        internal IEnumerable<ISocialMediaItem> GetSocialFeed(XmlNodeList nodes)
        {
            //<SocialFeed Source="lastfmtrack">Austerity</SocialFeed>
            var returnList = new List<ISocialMediaItem>();

            try
            {
                foreach (XmlNode node in nodes)
                {
                    if (String.IsNullOrEmpty(node.InnerText))
                        continue;

                    var newFeed = Kernel.Get<SocialMediaFeed>();
                    newFeed.ProfileTarget = node.InnerText;
                    newFeed.AuthKey = node.GetAttribute<String>("authKey", String.Empty);
                    newFeed.TypeClass = node.GetAttribute<SocialMediaTypeClass>("source", SocialMediaTypeClass.news);
                    returnList.AddRange(newFeed.GetData());
                }
            }
            catch //dont fail for social items please
            {
            }

            return returnList;
        }
    }
}
