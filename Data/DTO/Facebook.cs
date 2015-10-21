using System;
using Ninject;
using Site.Data.API;

namespace Site.Data.DTO
{
    [Serializable]
    public class FacebookPost : IFacebookPost
    {
        [Inject]
        public IKernel Kernel { get; set; }

        #region IFacebook Members

        public string Title { get; set; }
        public string Url { get; set; }
        public string ID { get; set; }
        public string Description { get; set; }
        public DateTime Published { get; set; }

        public ISocialMediaItem AsSocialMediaItem()
        {
            var smitem = Kernel.Get<SocialMediaItem>();

            smitem.Body = String.Format("<a href=\"{0}\" rel=\"nofollow\" target=\"_blank\">{1}</a>", Url, Title);
            smitem.TypeClass = SocialMediaTypeClass.facebook;

            return smitem;
        }

        #endregion
    }
}
