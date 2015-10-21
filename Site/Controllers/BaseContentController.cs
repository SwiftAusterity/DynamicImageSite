using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Ninject;

using Site.Data.API.Repository;
using Site.Models;
using Site.ViewModels;

namespace Site.Controllers
{
    public class BaseContentController : Controller
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public UserDataProvider UserProvider { get; set; }

        [Inject]
        public IContentPageRepository ContentPageRepository { get; set; }

        protected virtual String ParentSection() { return String.Empty; }
        protected virtual String IndexTitle() { return String.Empty; }
        protected virtual String MainClass() { return "root"; }

        public virtual ActionResult Index(String section, String page, bool mobile)
        {
            var viewModel = BuildViewModel(section, page, mobile);

            if (viewModel == null)
                return new RedirectResult("/missing");

            return View(String.Format("{0}Index", mobile ? "Mobile_" : String.Empty), viewModel);
        }

        public virtual ActionResult IndexWithViewModel(BaseViewModel viewModel, String section, String page, bool mobile)
        {
            viewModel = BuildViewModel(viewModel, section, page, mobile);

            if (viewModel == null)
                return new RedirectResult("/missing");

            return View(String.Format("{0}Index", mobile ? "Mobile_" : String.Empty), viewModel);
        }

        internal BaseViewModel BuildViewModel(String section, String page, bool mobile)
        {
            if(mobile)
                return BuildViewModel(new MobileViewModel(Kernel, HttpContext), section, page, mobile);
            else
                return BuildViewModel(new BaseViewModel(Kernel, HttpContext), section, page, mobile);
        }

        internal BaseViewModel BuildViewModel(BaseViewModel viewModel, String section, String page, bool mobile)
        {
            var contentPage = ContentPageRepository.Get(String.Format("{0}{1}{2}/"
                                , String.IsNullOrEmpty(ParentSection()) ? String.Empty : "/" + ParentSection()
                                , String.IsNullOrEmpty(section) ? String.Empty : "/" + section
                                , String.IsNullOrEmpty(page) ? String.Empty : "/" + page), mobile);

            if (contentPage == null)
                return null;

            viewModel.IndexTitle = IndexTitle();
            viewModel.MainClass = MainClass();

            viewModel.ContentPage = contentPage;

            if (!viewModel.ContentPage.Url.Equals(viewModel.ContentPage.SectionUrl, StringComparison.InvariantCultureIgnoreCase))
                viewModel.MainClass += " " + viewModel.ContentPage.Url.Substring(1, viewModel.ContentPage.Url.Length - 2)
                                                                        .Replace("/", "_").ToLowerInvariant();

            if (!viewModel.ContentPage.Url.Equals(viewModel.ContentPage.SubSectionUrl, StringComparison.InvariantCultureIgnoreCase)
                && !viewModel.ContentPage.SectionUrl.Equals(viewModel.ContentPage.SubSectionUrl, StringComparison.InvariantCultureIgnoreCase))
                viewModel.MainClass += " subsection";

            if (mobile)
            {
                var mobileViewModel = (MobileViewModel)viewModel;
                var contentMap = ContentPageRepository.Get(mobile).ToList();

                mobileViewModel.SectionPages = contentMap.Where(content =>
                                                            content.SectionUrl.Equals(contentPage.SectionUrl)
                                                            && content.Url == content.SubSectionUrl);

                return mobileViewModel;
            }

            return viewModel;
        }
    }
}
