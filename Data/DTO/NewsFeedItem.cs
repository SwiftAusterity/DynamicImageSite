using System;
using Ninject;
using Site.Data.API;

namespace Site.Data.DTO
{
    [Serializable]
    public class NewsFeedItem : INewsFeedItem
    {
        [Inject]
        public IKernel Kernel { get; set; }

        #region INewsFeedItem Members

        public string SourceTitle { get; set; }
        public string SourceUrl { get; set; }
        public string SourceDescription { get; set; }
        public string UID { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Published { get; set; }

        public ISocialMediaItem AsSocialMediaItem()
        {
            var smitem = Kernel.Get<SocialMediaItem>();

            smitem.Body = string.Format("<a href=\"{0}\" rel=\"nofollow\" target=\"_blank\">{1}</a>", Url, Title);
            smitem.TypeClass = SocialMediaTypeClass.news;

            return smitem;
        }

        #endregion
    }
}
