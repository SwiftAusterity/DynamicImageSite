using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Web.Configuration;

using Site.Data.API;
using Site.Data.API.Repository;
using Site.Data.DTO;

using Infuz.Utilities;

using Newtonsoft.Json.Linq;

using Ninject;

using Site.Data.Ajax;

namespace Site.Data.Live
{
    public class NewsFeedRepository : AjaxRepository, INewsFeedRepositoryBackingStore
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public INewsFeedRepository Me { get; set; }

        #region INewsFeedRepository Members

        public IEnumerable<INewsFeedItem> GetLatestFeed(string endpoint, bool initial)
        {
            var targetUrl = new Uri(endpoint);

            var data = UntilDovesCry<INewsFeedItem>(initial
                , targetUrl
                , "rss"
                , "channel"
                , (node, results) => AppendItem(node, results));

            return data;
        }

        #endregion


        #region INewsFeedRepository Members

        public IEnumerable<INewsFeedItem> GetLatestFeed(string endpoint)
        {
            return GetLatestFeed(endpoint, true);
        }

        #endregion

        private void AppendItem(XmlNode node, IList<INewsFeedItem> list)
        {
            var data = ReadChannel(node);
            foreach (INewsFeedItem item in data)
                list.Add(item);
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
        internal IEnumerable<INewsFeedItem> ReadChannel(XmlNode node)
        {
            if (node == null)
                return null;

            var itemList = new List<INewsFeedItem>();

            var sourceDescription = node.SelectSingleNode("description").InnerText;
            var sourceTitle = node.SelectSingleNode("title").InnerText;
            var sourceUrl = node.SelectSingleNode("link").InnerText;
            var items = node.SelectNodes("item");

            foreach (XmlNode item in items)
            {
                var newItem = ReadItem(item);
                newItem.SourceDescription = sourceDescription;
                newItem.SourceTitle = sourceTitle;
                newItem.SourceUrl = sourceUrl;
                itemList.Add(newItem);
            }

            return itemList;
        }

        internal INewsFeedItem ReadItem(XmlNode node)
        {
            if (node == null)
                return null;

            var description = node.SelectSingleNode("description").InnerText;
            var published = node.SelectSingleNode("pubDate").InnerText;
            var title = node.SelectSingleNode("title").InnerText;
            var uid = node.SelectSingleNode("guid").InnerText;
            var url = node.SelectSingleNode("link").InnerText;

            var pubDate = DateTime.MaxValue;
            if(!DateTime.TryParse(published, out pubDate))
                pubDate = DateTime.MaxValue;

            var item = Kernel.Get<NewsFeedItem>();
            item.Description = description;
            item.Published = pubDate;
            item.Title = title;
            item.UID = uid;
            item.Url = url;

            return item;
        }
    }
}