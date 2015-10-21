using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

using Site.Models;
using Site.Data.API;
using Site.Data.API.Repository;

using Ninject;

namespace Site.ViewModels
{
    public class BaseViewModel
    {
        [Inject]
        public ILastFMRepository LastFMRepository { get; set; }

        [Inject]
        public ITwitterRepository TwitterRepository { get; set; }

        [Inject]
        public INewsFeedRepository NewsRepository { get; set; }

        [Inject]
        public IFacebookRepository FacebookRepository { get; set; }

        public BaseViewModel(IKernel kernel, HttpContextBase context)
        {
            kernel.Inject(this);
            Kernel = kernel;
            Context = context;
            SocialMediaRotatorItems = new List<ISocialMediaItem>();

            // most recent albums
            var latestAlbums = LastFMRepository.GetLatestAlbums("Infuz");
            if (latestAlbums != null)
                SocialMediaRotatorItems.AddRange(latestAlbums.Where(item => item != null).Take(3).Select(album => album.ToSocialMediaItem()));

            // most recent tracks
            var latestTracks = LastFMRepository.GetLatestTracks("infuzaudio");
            if (latestTracks != null)
                SocialMediaRotatorItems.AddRange(latestTracks.Where(item => item != null).Take(3).Select(track => track.ToSocialMediaItem()));

            //Infuz RSS, only stuff newer than 2 weeks ago
            var newses = NewsRepository.GetLatestFeed("http://www.infuz.com/feed");
            if (newses != null)
                SocialMediaRotatorItems.AddRange(newses.Where(item => item != null).Where(item => item.Published.AddDays(14) > DateTime.Now).Select(news => news.ToSocialMediaItem()));

            //Infuz account twitters
            var twitters = TwitterRepository.GetLatestTweet("Infuz");
            if (twitters != null)
                SocialMediaRotatorItems.AddRange(twitters.Where(item => item != null).Where(twitter => twitter.CreatedAt.AddDays(14) > DateTime.Now).Select(twt => twt.ToSocialMediaItem()));

            //Infuz account twitters
            var posts = FacebookRepository.GetLatestFeed("infuzyourbrand", "224336427647940|7MKzzSKXRWiWdzlPNYWZ5iaW_3o");
            if (posts != null)
                SocialMediaRotatorItems.AddRange(posts.Where(item => item != null).Where(item => item.Published.AddDays(7) > DateTime.Now).Select(fb => fb.ToSocialMediaItem()));

            if (_errors == null)
                _errors = new List<string>();
            if (_messages == null)
                _messages = new List<string>();
        }

        public IKernel Kernel { get; private set; }
        public HttpContextBase Context { get; private set; }
        public Guid UserID { get; private set; }
        public string UserScreenName { get; private set; }
        public string PartialPathName { get; set; }
        public IContentPage ContentPage { get; set; }
        public List<ISocialMediaItem> SocialMediaRotatorItems { get; set; }
        public ISocialMediaItem PersonSocialMediaItem { get; set; }
        public string IndexTitle { get; set; }
        public string MainClass { get; set; }

        public string GoogleAnalyticsCode
        {
            get
            {
                var code = WebConfigurationManager.AppSettings["GoogleAnalyticsCode"];

                return code;
            }
        }

        private IList<string> _errors;
        public IList<string> Errors
        {
            get
            {
                if (_errors == null)
                    _errors = new List<string>();

                return _errors;
            }
            set { _errors = value; }
        }

        private IList<string> _messages;
        public IList<string> Messages
        {
            get
            {
                if (_messages == null)
                    _messages = new List<string>();

                return _messages;
            }
            set { _messages = value; }
        }
    }
}
