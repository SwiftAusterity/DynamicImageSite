using System;
using Ninject;
using Site.Data.API;

namespace Site.Data.DTO
{
    [Serializable]
    public class Tweet : ITweet
    {
        [Inject]
        public IKernel Kernel { get; set; }

        #region ITweet Members

        public string Url { get; set; }
        public string ExpandedUrl { get; set; }
        public string DisplayUrl { get; set; }
        public string UserName { get; set; }
        public long UserID { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public ISocialMediaItem AsSocialMediaItem()
        {
            var smitem = Kernel.Get<SocialMediaItem>();

            smitem.Body = Body.Replace("<em>", String.Empty).Replace("</em>", String.Empty);
            smitem.TypeClass = SocialMediaTypeClass.twitter;

            return smitem;
        }

        #endregion
    }
}
