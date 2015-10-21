using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API
{
    public interface ISocialMediaItem
    {
        string Body { get; set; }
        SocialMediaTypeClass TypeClass { get; set; }

        string AsHtml(bool hidden);
    }

    public interface ISocialMediaFeed
    {
        string ProfileTarget { get; set; }
        string AuthKey { get; set; }
        SocialMediaTypeClass TypeClass { get; set; }

        IEnumerable<ISocialMediaItem> GetData();
    }

    public enum SocialMediaTypeClass
    {
        twitter,
        news,
        lastfmtrack,
        lastfmalbum,
        facebook,
        tumblr,
        linkedin,
        pinterest,
        flickr,
        behance,
        quora,
        gplus,
        yelp,
        vimeo,
        youtube,
        pandora,
        foursquare,
        spotify,
        goodreads
    }
}
