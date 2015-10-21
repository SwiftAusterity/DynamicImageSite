using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Configuration;

using Site.Data.API;
using Site.Data.API.Repository;
using Site.Data.DTO;

using Infuz.Utilities;

using Newtonsoft.Json.Linq;

using Ninject;

using Site.Data.Ajax;
using Facebook;

namespace Site.Data.Live
{
    public class FacebookRepository : AjaxRepository, IFacebookRepositoryBackingStore
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public IFacebookRepository Me { get; set; }

        #region IFacebookRepositoryBackingStore Members

        public IEnumerable<IFacebookPost> GetLatestFeed(string source, string accessToken, bool initial)
        {
            var result = new List<IFacebookPost>();

            try
            {
                var client = new FacebookClient();
                client.AccessToken = accessToken;

                var person = (IDictionary<string, object>)client.Get(source);
                var feed = (IDictionary<string, object>)client.Get(source + "/posts");

                var jO = JObject.Parse(feed.ToString());
                var jT = jO["data"];

                var nodeList = jT.Children();

                foreach (JToken currentRow in nodeList)
                    AppendData(currentRow, result);

                return result;
            }
            catch
            {
            }

            return Enumerable.Empty<IFacebookPost>();
        }

        #endregion

        #region IFacebookRepository Members

        public IEnumerable<IFacebookPost> GetLatestFeed(string source, string accessToken)
        {
            return GetLatestFeed(source, accessToken, true);
        }

        #endregion


        private void AppendData(JToken jsonToken, IList<IFacebookPost> list)
        {
            var data = ReadFromJson(jsonToken);
            list.Add(data);
        }

        internal IFacebookPost ReadFromJson(JToken jT)
        {
            if (jT == null)
                return null;

            //url = http://www.facebook.com/{id.1}/posts/{id.2}

            var urlIDKey = GetJsonProperty<String>(jT, "id", String.Empty);
            var description = GetJsonProperty<String>(jT, "message", String.Empty);
            var title = GetJsonProperty<String>(jT, "name", String.Empty);
            var published = GetJsonProperty<DateTime>(jT, "created_time", DateTime.MinValue);

            urlIDKey = urlIDKey.Replace("_", "/posts/");

            var item = Kernel.Get<FacebookPost>();
            item.Url = "http://www.facebook.com/" + urlIDKey;
            item.Description = description;
            item.Title = title;
            item.Published = published;

            return item;
        }

        /*
{
    "data": [
        {
          "id": "100000482417709_296940226998286",
          "from": {
            "name": "Danny Nissenfeld",
            "id": "100000482417709"
          },
          "message": "ENTER THIS.",
          "picture": "http://platform.ak.fbcdn.net/www/app_full_proxy.php?app=159171887509265&v=1&size=z&cksum=dd417be57e27e636a6a33493944ea69c&src=https\u00253A\u00252F\u00252Fs3.amazonaws.com\u00252Fwildfire_production\u00252Ffeed_banners\u00252F73810\u00252FD3_Sweepstakes-Thumb.jpg",
          "link": "http://apps.Facebook.com/diabloiiibetasweeps/contests/159910/entries/new?referral_feed_id=44486188",
          "name": "Diablo III Beta Key Sweepstakes",
          "caption": "Danny entered the Diablo III Beta Key Sweepstakes for a chance to win a Diablo III Beta Key.",
          "icon": "http://photos-e.ak.fbcdn.net/photos-ak-snc1/v85006/29/159171887509265/app_2_159171887509265_6319.gif",
          "actions": [
            {
              "name": "Comment",
              "link": "http://www.Facebook.com/100000482417709/posts/296940226998286"
            },
            {
              "name": "Like",
              "link": "http://www.Facebook.com/100000482417709/posts/296940226998286"
            },
            {
              "name": "Enter to Win",
              "link": "http://apps.Facebook.com/diabloiiibetasweeps/contests/159910/entries/new?referral_feed_id=44486188"
            }
          ],
          "privacy": {
            "description": "Public",
            "value": "EVERYONE",
            "allow": "0",
            "deny": "0"
          },
          "type": "link",
          "application": {
            "name": "Diablo III Beta Key Sweepstakes",
            "canvas_name": "diabloiiibetasweeps",
            "namespace": "diabloiiibetasweeps",
            "id": "159171887509265"
          },
          "created_time": "2011-10-13T15:21:37+0000",
          "updated_time": "2011-10-13T15:21:37+0000",
          "comments": {
            "count": 0
          }
        },
      ],
  "paging": {
    "previous": "https://graph.Facebook.com/100000482417709/posts?format=json&limit=25&since=1318519297&__previous=1",
    "next": "https://graph.Facebook.com/100000482417709/posts?format=json&limit=25&until=1302218727"
  }
}
    */
    }
}