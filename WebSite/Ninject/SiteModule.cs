using System;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Infuz.API.SQL;
using Infuz.SQL;

using Ninject.Modules;

using Site.Data;
using Site.Data.API;
using Site.Data.API.Repository;
using Site.Data.Cached;
using Site.Models;

namespace Site.Ninject
{
    public class SiteModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISqlServiceProvider>()
                .ToMethod(context => new SqlServiceProvider(WebConfigurationManager.ConnectionStrings["Site"].ConnectionString)).InSingletonScope();

            // use the once-per-request cache when you are running a debugger so we don't have background refills going.
            if (Debugger.IsAttached)
                Bind<ICache>().To<HttpCacheNoRefillShim>().InSingletonScope();
            else
                Bind<ICache>().To<HttpCacheShim>().InSingletonScope();

            // Set the backing store repos
            Bind<IContactRepositoryBackingStore>().To<Data.Live.ContactRepository>().InSingletonScope();
            Bind<ITextCommandRepositoryBackingStore>().To<Data.Live.TextCommandRepository>().InSingletonScope();
            Bind<IContentPageRepositoryBackingStore>().To<Data.Live.ContentPageRepository>().InSingletonScope();
            Bind<IContentEntryRepositoryBackingStore>().To<Data.Live.ContentEntryRepository>().InSingletonScope();
            Bind<IUserRepositoryBackingStore>().To<Site.Data.Live.UserRepository>().InSingletonScope();
            Bind<ILastFMRepositoryBackingStore>().To<Site.Data.Live.LastFMRepository>().InSingletonScope();
            Bind<ITwitterRepositoryBackingStore>().To<Site.Data.Live.TwitterRepository>().InSingletonScope();
            Bind<INewsFeedRepositoryBackingStore>().To<Site.Data.Live.NewsFeedRepository>().InSingletonScope();
            Bind<IFacebookRepositoryBackingStore>().To<Site.Data.Live.FacebookRepository>().InSingletonScope();
            // Set the Repo's that the caching repos fallback to
            Bind<IContactRepository>().To<Site.Data.Cached.ContactRepository>().InSingletonScope();
            Bind<ITextCommandRepository>().To<Site.Data.Cached.TextCommandRepository>().InSingletonScope();
            Bind<IContentPageRepository>().To<Site.Data.Cached.ContentPageRepository>().InSingletonScope();
            Bind<IContentEntryRepository>().To<Site.Data.Cached.ContentEntryRepository>().InSingletonScope();
            Bind<IUserRepository>().To<Site.Data.Cached.UserRepository>().InSingletonScope();
            Bind<ILastFMRepository>().To<Site.Data.Cached.LastFMRepository>().InSingletonScope();
            Bind<ITwitterRepository>().To<Site.Data.Cached.TwitterRepository>().InSingletonScope();
            Bind<INewsFeedRepository>().To<Site.Data.Cached.NewsFeedRepository>().InSingletonScope();
            Bind<IFacebookRepository>().To<Site.Data.Cached.FacebookRepository>().InSingletonScope();

            // Set the objects we'll be using. Transient means they're instantiated new every time.
            Bind<ITextCommand>().To<Data.DTO.TextCommand>().InTransientScope();
            Bind<IFacebookPost>().To<Data.DTO.FacebookPost>().InTransientScope();
            Bind<INewsFeedItem>().To<Data.DTO.NewsFeedItem>().InTransientScope();
            Bind<ILastFMTrack>().To<Data.DTO.LastFMTrack>().InTransientScope();
            Bind<ILastFMAlbum>().To<Data.DTO.LastFMAlbum>().InTransientScope();
            Bind<ITweet>().To<Data.DTO.Tweet>().InTransientScope();
            Bind<ISocialMediaItem>().To<Data.DTO.SocialMediaItem>().InTransientScope();
            Bind<ISocialMediaFeed>().To<Data.DTO.SocialMediaFeed>().InTransientScope();
            Bind<INavElement>().To<Data.DTO.NavElement>().InTransientScope();
            Bind<IContentPage>().To<Data.DTO.ContentPage>().InTransientScope();
            Bind<IContentEntry>().To<Data.DTO.ContentEntry>().InTransientScope();
            Bind<IContentSection>().To<Data.DTO.ContentSection>().InTransientScope();
            Bind<IContentSubSection>().To<Data.DTO.ContentSubSection>().InTransientScope();
            Bind<IContact>().To<Data.DTO.Contact>().InTransientScope();
            Bind<IUser>().To<Site.Data.DTO.User>().InTransientScope();
        }
    }
}
