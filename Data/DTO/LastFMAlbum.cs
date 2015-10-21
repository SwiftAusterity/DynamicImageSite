using System;
using Ninject;
using Site.Data.API;

namespace Site.Data.DTO
{
    [Serializable]
    public class LastFMAlbum : ILastFMAlbum
    {
        [Inject]
        public IKernel Kernel { get; set; }

        #region ILastFMAlbum Members

        public string Name { get; set; }
        public string ArtistName { get; set; }
        public string Url { get; set; }
        public int PlayCount { get; set; }
 
        public ISocialMediaItem AsSocialMediaItem()
        {
            var smitem = Kernel.Get<SocialMediaItem>();

            smitem.Body = String.Format("<a href=\"{0}\" rel=\"nofollow\" target=\"_blank\">{1} by {2}</a>", Url, Name, ArtistName);
            smitem.TypeClass = SocialMediaTypeClass.lastfmalbum;

            return smitem;
        }
        #endregion
    }
}
