using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Ninject;

using Site.Data.API;
using Site.Data.API.Repository;
using Site.Models;
using Site.ViewModels;

namespace Site.Controllers
{
    [HandleError]
    public class RequestedContentController : Controller
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public UserDataProvider UserProvider { get; set; }

        [Inject]
        public IContentPageRepository ContentPageRepository { get; set; }

        //This whole thing is AJAX, and we only really need one method for getting anything.
        [OutputCache(CacheProfile = "requestedContent", VaryByParam = "*")]
        public ActionResult GetContent(string path, string fromPath, string callback)
        {
            var direction = string.Empty;
            var viewModel = new BaseViewModel(Kernel, HttpContext);
            path = path.Replace("_", "/");

            var mobile = path.StartsWith("/m");

            var contentPage = ContentPageRepository.Get(path, mobile);
            var contentMap = ContentPageRepository.Get(mobile).ToList();

            if (contentPage != null)
            {
                if (contentPage.Url.Equals("/Contact/Work-With-Us/", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (mobile)
                        viewModel = new ContactFormMobileViewModel(Kernel, HttpContext);
                    else
                        viewModel = new ContactFormViewModel(Kernel, HttpContext);
                }

                if (!string.IsNullOrEmpty(fromPath) && !fromPath.Equals("Undefined", StringComparison.InvariantCultureIgnoreCase))
                {
                    fromPath = fromPath.Replace("_", "/");
                    var fromContentPage = ContentPageRepository.Get(fromPath, mobile);
                    //figure out the direction

                    if (fromContentPage != null && fromContentPage.SectionUrl.Equals(contentPage.SectionUrl, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //Same section/subsection
                        //Transition direction is opposite the relative position of the new page
                        if (contentPage.SubSectionUrl.Equals(fromContentPage.SubSectionUrl, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (contentMap.IndexOf(contentPage) > contentMap.IndexOf(fromContentPage))
                                direction = "Left";
                            else
                                direction = "Right";
                        } //easy to assess this as the index is just greater
                        else
                        {
                            if (contentMap.IndexOf(contentPage) > contentMap.IndexOf(fromContentPage))
                                direction = "Up";
                            else
                                direction = "Down";
                        }
                    }
                }

                var htmlString = string.Empty;
                viewModel.ContentPage = contentPage;
                if (mobile)
                {
                    viewModel = new MobileViewModel(Kernel, HttpContext);

                    var sectionPages = contentMap.Where(content =>
                                            content.SectionUrl.Equals(contentPage.SectionUrl)
                                            && content.Url == content.SubSectionUrl);

                    foreach (IContentPage page in sectionPages)
                    {
                        viewModel.ContentPage = page;
                        htmlString += RenderHtml(page.PartialLocation, viewModel);
                    }
                }
                else
                    htmlString = RenderHtml(contentPage.PartialLocation, viewModel);

                return new JsonPResult(callback,
                       Json(new
                       {
                           html = htmlString,
                           title = contentPage.Title,
                           meta = contentPage.Meta,
                           backgrounds = contentPage.Backgrounds,
                           subNav = contentPage.SubNav,
                           sectionNav = contentPage.SectionNav,
                           forwardNav = contentPage.ForwardNav,
                           backwardNav = contentPage.BackwardNav,
                           primaryNav = contentPage.PrimaryNav,
                           url = contentPage.Url,
                           area = contentPage.SectionName,
                           section = contentPage.SubSectionName,
                           direction = direction,
                           slideshow = contentPage.SubSectionSlideshow,
                           infoText = contentPage.InfoText
                       }));
            }

            return new JsonPResult(callback, Json(new { success = false, error = "Invalid content identifier." }));
        }

        [OutputCache(CacheProfile = "requestedContent", VaryByParam = "*")]
        public ActionResult GetContentSection(string path, string fromPath, string callback)
        {
            var direction = string.Empty;
            var viewModel = new BaseViewModel(Kernel, HttpContext);
            path = path.Replace("_", "/");

            var mobile = false;
            if (path.StartsWith("/m"))
            {
                path = path.Substring(2, path.Length - 2);
                mobile = true;
            }

            var contentPage = ContentPageRepository.Get(path, mobile);

            if (contentPage != null)
            {
                if (contentPage.Url.Equals("/Contact/Work-With-Us/", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (mobile)
                        viewModel = new ContactFormMobileViewModel(Kernel, HttpContext);
                    else
                        viewModel = new ContactFormViewModel(Kernel, HttpContext);
                }

                var contentMap = ContentPageRepository.Get(mobile).ToList();

                if (!string.IsNullOrEmpty(fromPath) && !fromPath.Equals("Undefined", StringComparison.InvariantCultureIgnoreCase))
                {
                    fromPath = fromPath.Replace("_", "/");
                    var fromContentPage = ContentPageRepository.Get(fromPath, mobile);
                    //figure out the direction

                    if (fromContentPage != null && fromContentPage.SectionUrl.Equals(contentPage.SectionUrl, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //Same section/subsection
                        //Transition direction is opposite the relative position of the new page
                        if (contentPage.SubSectionUrl.Equals(fromContentPage.SubSectionUrl, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (contentMap.IndexOf(contentPage) > contentMap.IndexOf(fromContentPage))
                                direction = "Left";
                            else
                                direction = "Right";
                        } //easy to assess this as the index is just greater
                        else
                        {
                            if (contentMap.IndexOf(contentPage) > contentMap.IndexOf(fromContentPage))
                                direction = "Up";
                            else
                                direction = "Down";
                        }
                    }
                }

                if(mobile)
                    viewModel = new MobileViewModel(Kernel, HttpContext);

                var contentSection = contentMap.Where(content => content.SubSectionUrl.Equals(contentPage.SubSectionUrl));

                var sectionPages = new List<ContentPageJson>();

                foreach (IContentPage cpage in contentSection)
                {
                    viewModel.ContentPage = cpage;
                    var newSectionPage = new ContentPageJson(cpage, RenderHtml(cpage.PartialLocation, viewModel));

                    sectionPages.Add(newSectionPage);
                }

                return new JsonPResult(callback,
                                       Json(new
                                       {
                                           pages = sectionPages,
                                           direction = direction
                                       }));
            }

            return new JsonPResult(callback, Json(new { success = false, error = "Invalid content identifier." }));
        }

        [OutputCache(CacheProfile = "routes", VaryByParam = "*")]
        public ActionResult GetRoutes(bool mobile, string callback)
        {
            var contentPieces = ContentPageRepository.Get(mobile);

            return new JsonPResult(callback,
                                   Json(new
                                   {
                                       routes = contentPieces
                                   }));
        }

        private string RenderHtml(string pathName, BaseViewModel model)
        {
            return this.RenderPartialToString(pathName, model);
        }
    }
}
