using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Microsoft.ApplicationBlocks.ExceptionManagement;

using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;

using Site.Ninject;
using Site.Routing;

namespace Site
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override IKernel CreateKernel()
        {
            var modules = new INinjectModule[]
                                    {
                                        new SiteModule()
                                    };
            return new StandardKernel(modules);
        }

        protected override void OnApplicationStarted()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                int httpCode = 500;
                Exception lastError = Server.GetLastError();

                if (lastError is HttpException)
                    httpCode = ((HttpException)lastError).GetHttpCode();

                while (lastError != null && lastError.InnerException != null && lastError is HttpUnhandledException)
                    lastError = lastError.InnerException;

                Response.StatusCode = httpCode;

                if (httpCode == 404)
                    FourOhFour(Request.Path);

                if (httpCode == 500)
                {
                    //do some reporting
                    if (lastError != null && !(lastError is ThreadAbortException))
                        ExceptionManager.Publish(lastError);
                }
				
                Server.ClearError();
            }
            catch
            {
                // eat exceptions while publishing exceptions
            }

        }

        private void FourOhFour(string pageName)
        {
            Context.Response.Clear();
            Context.Response.ClearHeaders();
            Context.Response.ClearContent();
            Context.Response.AppendHeader("requestedPage", pageName);
            Context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
            Context.Response.RedirectLocation = "/missing";
            Context.Response.TrySkipIisCustomErrors = true;
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            // Ignore text, html, files.
            routes.IgnoreRoute("{file}.txt",
                new { categoryId = Kernel.Get<AllowedTextFilesConstraint>() });
            routes.IgnoreRoute("{file}.htm");
            routes.IgnoreRoute("{file}.html");

            // Ignore axd files such as assest, image, sitemap etc
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ignore the assets directory which contains images, js, css & html
            routes.IgnoreRoute("Content/{*pathInfo}");

            //Exclude favicon (google toolbar request gif file as fav icon which is weird).
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });

#if(DEBUG)
            routes.MapRoute("DebuggingHook"
                          , "{foo}/{bar}/{whatever}/{yeah}/{right}/{ok}/{then}/{sharks}"
                          , new
                          {
                              controller = "Root",
                              action = "Diagnostic",
                              foo = string.Empty,
                              bar = string.Empty,
                              whatever = string.Empty,
                              yeah = string.Empty,
                              right = string.Empty,
                              ok = string.Empty,
                              then = string.Empty,
                              sharks = string.Empty
                          }
                          , new
                          {
                              foo = new DiagnosticRouteConstraint()
                          });
#endif

            AddOldSiteRerouting(routes);

            routes.MapRoute(
                "CrawlerAjaxHandling",
                "",
                new { controller = "CrawlerAjax", action = "Handle" },
                new { foo = Kernel.Get<CrawlerAjaxRouteConstraint>() });

            routes.MapRoute(
                "sitemap",
                "sitemap.xml",
                new { controller = "Static", action = "Sitemap", mobile = false });

            routes.MapRoute(
                "sitemap_mobile",
                "m/sitemap.xml",
                new { controller = "Static", action = "Sitemap", mobile = true });

            routes.MapRoute(
                "IAmError",
                "error",
                new { controller = "Static", staticPage = "error", action = "ShowStaticContent" });

            routes.MapRoute(
                "WheresWaldo",
                "missing",
                new { controller = "Static", staticPage = "missing", action = "ShowStaticContent" });

            AddTextAdventureRouting(routes);
            AddContactRoutes(routes);
            AddContentAjaxRoutes(routes);
            AddMobileRoutes(routes);

            routes.MapRoute(
                "WorkRoot",
                "Work/{section}/{page}",
                new { controller = "Work", action = "Index", section = string.Empty, page = string.Empty, mobile = false }
            );

            routes.MapRoute(
                "Root",
                "{section}/{page}",
                new { controller = "Root", action = "Index", section = string.Empty, page = string.Empty, mobile = false },
                new
                  {
                      foo = new RootExclusionRouteConstraint()
                  }
            );

            //base route
            routes.MapRoute(
                "Fallthru",
                "{controller}/{section}/{page}",                        
                new { controller = "Root", action = "Index", section = string.Empty, page = string.Empty, mobile = false }
            );
        }

        private void AddMobileRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "WorkRootMobile",
                "m/Work/{section}/{page}",
                new { controller = "Work", action = "Index", section = string.Empty, page = string.Empty, mobile = true }
            );

            routes.MapRoute(
                "ContactRootMobile",
                "m/Contact/{section}/{page}",
                new { controller = "Contact", action = "Index", section = string.Empty, page = string.Empty, mobile = true }
            );

            routes.MapRoute(
                "RootMobile",
                "m/{section}/{page}",
                new { controller = "Root", action = "Index", section = string.Empty, page = string.Empty, mobile = true },
                new
                {
                    foo = new RootExclusionRouteConstraint()
                }
            );

            routes.MapRoute(
                "FallthruMobile",
                "m/{controller}/{section}/{page}",
                new { controller = "Root", action = "Index", section = string.Empty, page = string.Empty, mobile = true }
            );
        }

        private void AddContentAjaxRoutes(RouteCollection routes)
        {
            //AJAX get content routes
            routes.MapRoute(
                "AjaxGetRoutes",
                "Ajax/GetRoutes/{mobile}",
                new { controller = "RequestedContent", action = "GetRoutes", mobile = false });

            routes.MapRoute(
                "AjaxGetContent",
                "Ajax/GetContent/{path}/{fromPath}",
                new { controller = "RequestedContent", action = "GetContent", fromPath = string.Empty, mobile = false });

            routes.MapRoute(
                "AjaxGetContentSection",
                "Ajax/GetContentSection/{path}/{fromPath}",
                new { controller = "RequestedContent", action = "GetContentSection", fromPath = string.Empty, mobile = false });
        }

        private void AddOldSiteRerouting(RouteCollection routes)
        {
            routes.MapRoute(
               "Docs Route",
               "docs/{*pathInfo}",
               new { controller = "Reroute", action = "Docs" });

            routes.MapRoute(
               "Blog Route",
               "thinktank/{*pathInfo}",
               new { controller = "Reroute", action = "Blog" });

            routes.MapRoute(
               "2010 Blog Route",
               "2010/{*pathInfo}",
               new { controller = "Reroute", action = "BlogDate", year = 2010 });

            routes.MapRoute(
               "2011 Blog Route",
               "2011/{*pathInfo}",
               new { controller = "Reroute", action = "BlogDate", year = 2011 });

            routes.MapRoute(
               "Category Blog Route",
               "category/{*pathInfo}",
               new { controller = "Reroute", action = "BlogCategory" });

            routes.MapRoute(
               "Author Blog Route",
               "author/{*pathInfo}",
               new { controller = "Reroute", action = "BlogAuthor" });

            routes.MapRoute(
               "feed Blog Route",
               "feed/{*pathInfo}",
               new { controller = "Reroute", action = "BlogFeed" });

            routes.MapRoute(
               "Agency Route",
               "agency/{*pathInfo}",
               new { controller = "Reroute", action = "Agency" },
                new
                {
                    foo = new AgencyExclusionRouteConstraint()
                });

            routes.MapRoute(
               "Approach Route",
               "approach/{*pathInfo}",
               new { controller = "Reroute", action = "Approach" });

            routes.MapRoute(
               "SocialMedia Route",
               "social-media/{*pathInfo}",
               new { controller = "Reroute", action = "SocialMedia" });

            routes.MapRoute(
               "blog wp",
               "wp-content/{*pathInfo}",
               new { controller = "Reroute", action = "BlogWP" });
        }

        private void AddContactRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "AjaxSubmitContact",
                "ajax/contact/submit",
                new { controller = "Contact", action = "AjaxSubmitContact", enewsletter = false });

            routes.MapRoute(
                "AjaxSubmitSubscribe",
                "ajax/subscribe/submit",
                new { controller = "Contact", action = "AjaxSubmitSubscribe" });

            routes.MapRoute(
                "SubmitContact",
                "contact/submit",
                new { controller = "Contact", action = "SubmitContact", enewsletter = false });

            routes.MapRoute(
                "SubmitSubscribe",
                "contact/subscribe",
                new { controller = "Contact", action = "SubmitSubscribe" });

            routes.MapRoute(
                "MobileSubmitContact",
                "m/contact/submit",
                new { controller = "Contact", action = "SubmitContact", enewsletter = false });

            routes.MapRoute(
                "MobileSubmitSubscribe",
                "m/contact/subscribe",
                new { controller = "Contact", action = "SubmitSubscribe" });
        }

        private void AddTextAdventureRouting(RouteCollection routes)
        {
            routes.MapRoute(
               "TextADV_MakeCommand",
               "Ajax/textadv/{path}/{command}",
               new { controller = "TextAdventure", action = "MakeCommand", callback  = string.Empty, path = string.Empty, command = string.Empty });
            
            routes.MapRoute(
               "TextADV_Index",
               "textadv",
               new { controller = "TextAdventure", action = "Index" });
        }
    }
}