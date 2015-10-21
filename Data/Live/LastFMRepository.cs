using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using Newtonsoft.Json.Linq;
using Ninject;
using Site.Data.Ajax;
using Site.Data.API;
using Site.Data.API.Repository;
using Site.Data.DTO;

namespace Site.Data.Live
{
    public class LastFMRepository : AjaxRepository, ILastFMRepositoryBackingStore
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public ILastFMRepository Me { get; set; }

        protected string UrlBase
        {
            get
            {
                return WebConfigurationManager.AppSettings["LastFMTarget"];
            }
        }

        protected string APIKey
        {
            get
            {
                return WebConfigurationManager.AppSettings["LastFMAPIKey"];
            }
        }

        #region ILastFMRepositoryBackingStore Members

        public IEnumerable<ILastFMTrack> GetLatestTracks(string username, bool initial)
        {
            var targetUrl = new Uri(string.Format("{0}?method=user.getrecenttracks&user={1}&api_key={2}&format=json"
                        , UrlBase
                        , username
                        , APIKey));

            var data = UntilDovesCry<ILastFMTrack>(initial
                , targetUrl
                , "recenttracks"
                , "track"
                , (JToken, results) => AppendTrack(JToken, results));

            return data;
        }

        public IEnumerable<ILastFMAlbum> GetLatestAlbums(string groupName, bool initial)
        {
            var targetUrl = new Uri(string.Format("{0}?method=group.getweeklyalbumchart&group={1}&api_key={2}&format=json"
                        , UrlBase
                        , groupName
                        , APIKey));

            var data = UntilDovesCry<ILastFMAlbum>(initial
                , targetUrl
                , "weeklyalbumchart"
                , "album"
                , (JToken, results) => AppendAlbum(JToken, results));

            return data;
        }

        #endregion

        #region ILastFMRepository Members

        public IEnumerable<ILastFMTrack> GetLatestTracks(string username)
        {
            return GetLatestTracks(username, true);
        }

        public IEnumerable<ILastFMAlbum> GetLatestAlbums(string groupName)
        {
            return GetLatestAlbums(groupName, true);
        }

        #endregion

        private void AppendTrack(JToken jsonToken, IList<ILastFMTrack> list)
        {
            var link = ReadTrackFromJson(jsonToken);
            list.Add(link);
        }

        private void AppendAlbum(JToken jsonToken, IList<ILastFMAlbum> list)
        {
            var link = ReadAlbumFromJson(jsonToken);
            list.Add(link);
        }

        /*
{"weeklyalbumchart":
	{"album":
		{"artist":
			{"#text":"Woven Hand","mbid":"90f3ba7d-194e-4044-92be-10fc7843c4af"},
			"name":"Woven Hand",
			"mbid":"8278052f-9893-475f-9f57-35feb8a05995",
			"playcount":"1",
			"url":"http:\/\/www.last.fm\/music\/Woven+Hand\/Woven+Hand",
			"@attr":
                { "rank":"1" }
		},
		"@attr":
			{"group":"Infuz","from":"1325419200","to":"1326024000"}
	}
}
 */
        internal ILastFMAlbum ReadAlbumFromJson(JToken jT)
        {
            if (jT == null)
                return null;

            jT = jT.First();

            var artistNode = jT["artist"];

            var artistName = GetJsonProperty<String>(artistNode, "#text", string.Empty);
            var name = GetJsonProperty<String>(jT, "name", string.Empty);
            var url = GetJsonProperty<String>(jT, "url", string.Empty);
            var playCount = GetJsonProperty<int>(jT, "playcount", default(int));

            var item = Kernel.Get<LastFMAlbum>();
            item.ArtistName = artistName;
            item.Name = name;
            item.PlayCount = playCount;
            item.Url = url;

            return item;
        }

        /*
    <track nowplaying="true">
        <artist mbid="2f9ecbed-27be-40e6-abca-6de49d50299e">Aretha Franklin</artist>
        <name>Sisters Are Doing It For Themselves</name>
        <mbid/>
        <album mbid=""/>
        <url>www.last.fm/music/Aretha+Franklin/_/Sisters+Are+Doing+It+For+Themselves</url>
        <date uts="1213031819">9 Jun 2008, 17:16</date>
        <streamable>1</streamable>
    </track>
 */
        internal ILastFMTrack ReadTrackFromJson(JToken jT)
        {
            if (jT == null)
                return null;

            jT = jT.First();

            var artistNode = jT["artist"];
            var artistName = GetJsonProperty<String>(artistNode, "#text", string.Empty);

            var albumNode = jT["album"];
            var albumName = GetJsonProperty<String>(albumNode, "#text", string.Empty);

            var name = GetJsonProperty<String>(jT, "name", string.Empty);
            var nowPlaying = GetJsonProperty<bool>(jT, "nowplaying", false);
            var url = GetJsonProperty<String>(jT, "url", string.Empty);
            var dateNode = jT["date"];
            var date = GetJsonProperty<DateTime>(dateNode, "#text", DateTime.MaxValue);
            var streamable = GetJsonProperty<int>(jT, "streamable", default(int));

            var item = Kernel.Get<LastFMTrack>();
            item.Name = name;
            item.NowPlaying = nowPlaying;
            item.ArtistName = artistName;
            item.AlbumName = albumName;
            item.Url = url;
            item.Date = date;
            item.Streamable = streamable;

            return item;
        }
    }
}