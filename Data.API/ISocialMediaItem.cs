using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API
{
    public interface ISocialMediaItem
    {
        String Body { get; set; }
        SocialMediaTypeClass TypeClass { get; set; }

        String AsHtml(bool hidden);
    }

    public interface ISocialMediaFeed
    {
        String ProfileTarget { get; set; }
        String AuthKey { get; set; }
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
