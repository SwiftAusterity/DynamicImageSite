using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web.Mvc;

using Ninject;

using Site.Data.API.Repository;
using Site.Models;
using Site.ViewModels;

using Site.Data.API;

namespace Site.Controllers
{
    [HandleError(View = "Error")]
    public class StaticController : Controller
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public IContentPageRepository ContentPageRepository { get; set; }

        public ActionResult ShowStaticContent(string staticPage)
        {
            if (staticPage == "missing")
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
            }

            var viewModel = new ErrorViewModel();
            return View(staticPage, viewModel);
        }

        public ActionResult Gone(string Stuff)
        {
            Response.StatusCode = 410;
            Response.Status = "410 Gone";
            Response.TrySkipIisCustomErrors = true;

            var viewModel = new ErrorViewModel();
            return View("Gone", viewModel);
        }

        public XmlSitemapResult Sitemap(bool mobile)
        {
            var map = ContentPageRepository.Get(mobile);
            var items = new List<ISitemapItem>();

            foreach (IContentPage page in map.Where(pg => pg.Visible))
            {
                items.Add(
                    new SitemapItem("http://www.infuz.com" + page.Url)
                    {
                        Priority = page.SiteMapPriority
                    }
                );
            }

            return new XmlSitemapResult(items);
        }
    }
}
