using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Site.Data.API;
using Site.Data.API.Repository;

namespace Site.Data.DTO
{
    [Serializable]
    public class SocialMediaItem : ISocialMediaItem
    {
        #region ISocialMediaItem Members

        public string Body { get; set; }
        public SocialMediaTypeClass TypeClass { get; set; }

        public string AsHtml(bool hidden)
        {
            var html = new StringBuilder();

            if (hidden)
                html.AppendFormat("<div class=\"slide excerpt {0}\" style=\"display: none\">", TypeClass.ToString());
            else
                html.AppendFormat("<div class=\"slide excerpt {0}\">", TypeClass.ToString());

            html.AppendFormat("<span class=\"icon icon-sm icon-{0}\"></span>", TypeClass.ToString());
            html.AppendFormat("<p>{0}</p>", Body);
            html.Append("</div>");

            return html.ToString();
        }

        #endregion
    }

    [Serializable]
    public class SocialMediaFeed : ISocialMediaFeed
    {
        [Inject]
        public ILastFMRepository LastFMRepository { get; set; }

        [Inject]
        public ITwitterRepository TwitterRepository { get; set; }

        [Inject]
        public IFacebookRepository FacebookRepository { get; set; }

        [Inject]
        public INewsFeedRepository NewsRepository { get; set; }

        public string ProfileTarget { get; set; }
        public string AuthKey { get; set; }
        public SocialMediaTypeClass TypeClass { get; set; }

        public IEnumerable<ISocialMediaItem> GetData()
        {
            var mediaItems = new List<ISocialMediaItem>();

            if (!string.IsNullOrEmpty(ProfileTarget))
                switch (TypeClass)
                {
                    case SocialMediaTypeClass.lastfmalbum:
                        var albums = LastFMRepository.GetLatestAlbums(ProfileTarget);
                        mediaItems.AddRange(albums.Where(item => item != null).Take(3).Select(album => album.ToSocialMediaItem()));
                        break;
                    case SocialMediaTypeClass.lastfmtrack:
                        var tracks = LastFMRepository.GetLatestTracks(ProfileTarget);
                        if (tracks != null)
                            mediaItems.AddRange(tracks.Where(item => item != null).Take(5).Select(track => track.ToSocialMediaItem()));
                        break;
                    case SocialMediaTypeClass.twitter:
                        var tweets = TwitterRepository.GetLatestTweet(ProfileTarget);
                        if (tweets != null)
                            mediaItems.AddRange(tweets.Where(item => item.CreatedAt.AddDays(7) > DateTime.Now).Where(item => item != null).Select(data => data.ToSocialMediaItem()));
                        break;
                    case SocialMediaTypeClass.news:
                        var news = NewsRepository.GetLatestFeed(ProfileTarget);
                        if (news != null)
                            mediaItems.AddRange(news.Where(item => item.Published.AddDays(7) > DateTime.Now).Where(item => item != null).Select(data => data.ToSocialMediaItem()));
                        break;
                    case SocialMediaTypeClass.facebook:
                        var posts = FacebookRepository.GetLatestFeed(ProfileTarget, AuthKey);
                        if (posts != null)
                            mediaItems.AddRange(posts.Where(item => item.Published.AddDays(7) > DateTime.Now).Where(item => item != null).Select(data => data.ToSocialMediaItem()));
                        break;
                }

            return mediaItems;
        }
    }
}
