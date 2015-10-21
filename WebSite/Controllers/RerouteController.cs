using System;
using System.Web.Mvc;

namespace Site.Controllers
{
    [HandleError]
    public class RerouteController : Controller
    {
        public ActionResult Docs(string pathInfo)
        {
            return new RedirectResult("/Content/media/" + pathInfo);
        }

        public ActionResult Blog(string pathInfo)
        {
            return new RedirectResult("http://blog.infuz.com/" + pathInfo);
        }

        public ActionResult BlogWP(string pathInfo)
        {
            return new RedirectResult("http://blog.infuz.com/wp-content/" + pathInfo);
        }

        public ActionResult BlogDate(string pathInfo, int year)
        {
            return new RedirectResult("http://blog.infuz.com/" + year.ToString() + "/" + pathInfo);
        }

        public ActionResult BlogCategory(string pathInfo)
        {
            return new RedirectResult("http://blog.infuz.com/category/" + pathInfo);
        }

        public ActionResult BlogAuthor(string pathInfo)
        {
            return new RedirectResult("http://blog.infuz.com/author/" + pathInfo);
        }

        public ActionResult BlogFeed(string pathInfo)
        {
            return new RedirectResult("http://blog.infuz.com/feed/" + pathInfo);
        }

        public ActionResult Agency(string pathInfo)
        {
            var target = "/Agency/";

            if (!string.IsNullOrEmpty(pathInfo))
            {
                if (pathInfo.StartsWith("careers/junior-copywriter", StringComparison.InvariantCultureIgnoreCase)
                    || pathInfo.StartsWith("careers/senior-account-executive", StringComparison.InvariantCultureIgnoreCase)
                    || pathInfo.StartsWith("careers/senior-user-experience-designer-architect", StringComparison.InvariantCultureIgnoreCase))
                    target = "/Agency/Careers/";
                if (pathInfo.StartsWith("the-space", StringComparison.InvariantCultureIgnoreCase))
                    target = "/";
                if (pathInfo.StartsWith("culture", StringComparison.InvariantCultureIgnoreCase))
                    target = "/People/";
                if (pathInfo.StartsWith("services", StringComparison.InvariantCultureIgnoreCase))
                    target = "/Expertise/";

                if (pathInfo.StartsWith("leadership", StringComparison.InvariantCultureIgnoreCase))
                {
                    target = "/People/Leadership/";

                    if (pathInfo.StartsWith("leadership/hafiz-huda", StringComparison.InvariantCultureIgnoreCase))
                        target = "/People/Leadership/hafiz-huda/";
                    if (pathInfo.StartsWith("leadership/jason-fiehler", StringComparison.InvariantCultureIgnoreCase))
                        target = "/People/Leadership/jason-fiehler/";
                    if (pathInfo.StartsWith("leadership/jill-schanzle", StringComparison.InvariantCultureIgnoreCase))
                        target = "/People/Leadership/jill-schanzle/";
                    if (pathInfo.StartsWith("leadership/katie-odell", StringComparison.InvariantCultureIgnoreCase))
                        target = "/People/Leadership/katie-odell/";
                    if (pathInfo.StartsWith("leadership/ryan-stephenson", StringComparison.InvariantCultureIgnoreCase))
                        target = "/People/Leadership/ryan-stephenson/";
                }
            }

            return new RedirectResult(target);
        }

        public ActionResult Approach(string pathInfo)
        {
            var target = "/Expertise/";

            if (!string.IsNullOrEmpty(pathInfo))
            {
                if (pathInfo.StartsWith("our-process", StringComparison.InvariantCultureIgnoreCase))
                    target = "/Expertise/Process/";
            }
            return new RedirectResult(target);
        }

        public ActionResult SocialMedia(string pathInfo)
        {
            var target = "/Expertise/";

            if (!string.IsNullOrEmpty(pathInfo))
            {
                if (pathInfo.StartsWith("relationship-science", StringComparison.InvariantCultureIgnoreCase))
                    target = "/Expertise/Relationship-Architecture/";
            }

            return new RedirectResult(target);
        }
    }
}
