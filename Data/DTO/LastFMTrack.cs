using System;
using Ninject;
using Site.Data.API;

namespace Site.Data.DTO
{
    [Serializable]
    public class LastFMTrack : ILastFMTrack
    {
        [Inject]
        public IKernel Kernel { get; set; }

        #region ILastFMTrack Members

        public string Name { get; set; }
        public bool NowPlaying { get; set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public int Streamable { get; set; }

        public ISocialMediaItem AsSocialMediaItem()
        {
            var smitem = Kernel.Get<SocialMediaItem>();

            smitem.Body = String.Format("<a href=\"{0}\" rel=\"nofollow\" target=\"_blank\">{1} by {3}</a>", Url, Name, AlbumName, ArtistName);
            smitem.TypeClass = SocialMediaTypeClass.lastfmtrack;

            return smitem;
        }
            
        #endregion
    }
}
