using System;
using System.Web.Mvc;

using Ninject;

using Site.Models;

namespace Site.Controllers
{
    [HandleError]
    public class CrawlerAjaxController : Controller
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public UserDataProvider UserProvider { get; set; }

        //unlogged home
        public ActionResult Handle(String _escaped_Fragment_)
        {
            var newRoute = String.Format("/{0}", _escaped_Fragment_);
            return new RedirectResult(_escaped_Fragment_);
        }
    }
}
