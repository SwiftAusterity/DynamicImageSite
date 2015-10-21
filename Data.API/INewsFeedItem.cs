using System;

namespace Site.Data.API
{
    public interface INewsFeedItem
    {
        string SourceTitle { get; set; }
        string SourceUrl { get; set; }
        string SourceDescription { get; set; }

        string UID { get; set; }
        string Title { get; set; }
        string Url { get; set; }
        string Description { get; set; }
        DateTime Published { get; set; }

        ISocialMediaItem AsSocialMediaItem();
    }

    public static partial class SMIHelper
    {
        public static ISocialMediaItem ToSocialMediaItem(this INewsFeedItem news)
        {
            return news == null ? null : news.AsSocialMediaItem();
        }
    }

    /*
    <?xml version="1.0" encoding="utf-8"?>

    <rss version="2.0">
    <channel>
    <title>xkcd.com</title>
    <link>http://xkcd.com/</link>
    <description>xkcd.com: A webcomic of romance and math humor.</description>
    <language>en</language>

    <item>
    <title>Batman</title>
    <link>http://xkcd.com/1004/</link>
    <description>&lt;img src="http://imgs.xkcd.com/comics/batman.png" title="I'm really worried Christopher Nolan will kill a man dressed like a bat in his next movie. (The man will be dressed like a bat, I mean. Christopher Nolan won't be, probably.)" alt="I'm really worried Christopher Nolan will kill a man dressed like a bat in his next movie. (The man will be dressed like a bat, I mean. Christopher Nolan won't be, probably.)" /&gt;</description>
    <pubDate>Mon, 16 Jan 2012 05:00:00 -0000</pubDate>
    <guid>http://xkcd.com/1004/</guid>
    </item>

    </channel>
    </rss>
    */
}
