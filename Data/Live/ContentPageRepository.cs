using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Linq;

using Infuz.API.SQL;
using Infuz.Utilities;

using Ninject;

using Site.Data.API;
using Site.Data.API.Repository;
using Site.Data.File;
using Site.Data.DTO;
using System.Web.Configuration;

namespace Site.Data.Live
{
    public class ContentPageRepository : FileRepository, IContentPageRepositoryBackingStore
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public IContentPageRepository Me { get; set; }

        [Inject]
        public IContentEntryRepository ContentEntryRepository { get; set; }

        protected string PageLocation
        {
            get
            {
                return WebConfigurationManager.AppSettings["PageFile"];
            }
        }

        #region IContentPageRepositoryBackingStore Members

        public IEnumerable<IContentPage> Get(bool mobile, bool initial)
        {
            var pageList = UntilDovesCry<IContentPage>(PageLocation
                                        , "Pages"
                                        , "Section"
                                        , (node, results) => AppendFromDataReader(node, results, mobile));

            //Still need to set the primary nav
            var primaryNav = new List<INavElement>();

            //Exclude the root page
            foreach (IContentPage page in pageList.Where(pg => pg.Visible && pg.SectionUrl == pg.Url && pg.Url != "/" && pg.Url != "/m/"))
                primaryNav.Add(GetOneNav(page.Url, page.Url, page.SectionName));

            foreach (IContentPage page in pageList)
                page.PrimaryNav = primaryNav;

            return pageList;
        }

        #endregion

        #region IContentPageRepository Members

        public bool Save(string url, string title, string meta, string partialLocation
            , IEnumerable<string> backgrounds, IEnumerable<INavElement> sectionNav, IEnumerable<INavElement> subNav
            , INavElement forwardNav, INavElement backwardNav)
        {
            throw new NotImplementedException();
        }

        public bool Delete(IContentPage contentPiece)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IContentPage> Get(bool mobile)
        {
            return Get(mobile, true);
        }

        public IContentPage Get(string url, bool mobile)
        {
            var list = Me.Get(mobile);

            if (mobile)
                url = url.StartsWith("/m") ? url : "/m" + url;

            return list.FirstOrDefault(content => content.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion

        private void AppendFromDataReader(XmlNode node, IList<IContentPage> result, bool mobile)
        {
            var data = ReadSection(node, mobile);
            foreach (IContentPage page in data)
                result.Add(page);
        }

        internal IList<IContentPage> ReadSection(XmlNode node, bool mobile)
        {
            if (node == null)
                return new List<IContentPage>();

            var mobileSection = node.GetAttribute<bool>("mobile", false);

            if (mobile && !mobileSection)
                return new List<IContentPage>();

            var sectionPages = node.SelectNodes("Page");
            var subSections = node.SelectNodes("SubSection");
            var sectionMeta = node.SelectSingleNode("Meta");
            var noSectionRootPage = node.GetAttribute<bool>("noRootPage", false);
            var sectionSlideshow = node.GetAttribute<bool>("slideshow", false);
            var sectionRootPageTitle = node.GetAttribute<String>("rootPageName", String.Empty);
            var sectionSiteMapPriority = node.GetAttribute<float>("siteMapPriority", 1);
            var sectionUrl = node.GetAttribute<String>("baseUrl", String.Empty);
            var sectionName = node.GetAttribute<String>("name", String.Empty);
            var sectionVisible = node.GetAttribute<bool>("visible", true);

            if (mobile)
                sectionUrl = "/m" + sectionUrl;

            var sectionNav = Enumerable.Empty<INavElement>();

            if (sectionVisible)
                sectionNav = GetSectionNav(sectionUrl, subSections
                                            , !String.IsNullOrEmpty(sectionRootPageTitle) ? sectionRootPageTitle : sectionName
                                            , noSectionRootPage, mobile);

            var section = new ContentSection
            {
                SectionSlideshow = sectionSlideshow,
                SectionRootPageTitle = sectionRootPageTitle,
                SectionMeta = sectionMeta.InnerText,
                SectionName = sectionName,
                SectionUrl = sectionUrl,
                SectionNav = sectionNav
            };

            var fakeSubNav = Enumerable.Empty<INavElement>();

            if (sectionVisible)
                fakeSubNav = GetSubNav(section.SectionUrl, sectionPages
                            , !String.IsNullOrEmpty(sectionRootPageTitle) ? sectionRootPageTitle : section.SectionName
                            , noSectionRootPage, mobile);

            //Need a "fake" subsection to pull in subsectionless pages
            var stubSubSection = new ContentSubSection
            {
                SubSectionSlideshow = sectionSlideshow,
                SubSectionRootPageTitle = section.SectionRootPageTitle,
                SubSectionMeta = section.SectionMeta,
                SubSectionName = String.Empty,
                SubSectionUrl = section.SectionUrl,
                SubNav = fakeSubNav,
                SectionSlideshow = section.SectionSlideshow,
                SectionRootPageTitle = section.SectionRootPageTitle,
                SectionMeta = section.SectionMeta,
                SectionName = section.SectionName,
                SectionUrl = section.SectionUrl,
                SectionNav = section.SectionNav
            };

            var sectionContentPageList = new List<IContentPage>();

            //Add a root page
            if (!noSectionRootPage)
            {
                var rootSectionPage = ReadPageFrom(section.SectionUrl, section.SectionUrl, stubSubSection, sectionSiteMapPriority, mobile, sectionVisible);

                if (sectionVisible)
                {
                    if (sectionPages.Count > 0 && sectionPages[0].Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                        rootSectionPage.ForwardNav = MakeNav(sectionPages[0], section.SectionUrl);

                    //Make revolving backwards nav
                    if (sectionPages.Count > 0 && sectionPages[sectionPages.Count - 1].Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                        rootSectionPage.BackwardNav = MakeNav(sectionPages[sectionPages.Count - 1], section.SectionUrl);
                }

                sectionContentPageList.Add(rootSectionPage);
            }

            foreach (XmlNode page in sectionPages)
            {
                var mobilePage = page.GetAttribute<bool>("mobile", false);

                if (mobile && !mobilePage)
                    continue;

                var visible = node.GetAttribute<bool>("visible", true);

                var contentSwitch = page.GetAttribute<bool>("explicitContent", false);
                var url = section.SectionUrl + page.InnerText;
                var pageKey = url;

                if (contentSwitch)
                    pageKey = page.InnerText;

                var pageSiteMapPriority = page.GetAttribute<float>("siteMapPriority", 1);

                var newPage = ReadPageFrom(pageKey, url, stubSubSection, pageSiteMapPriority, mobile, visible);

                if (visible)
                {
                    var parentNode = page.ParentNode;

                    if (page.PreviousSibling != null && page.PreviousSibling.Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                         newPage.BackwardNav = MakeNav(page.PreviousSibling, section.SectionUrl);
                    //This is a first page and we need to make a root nav
                    else if (!noSectionRootPage)
                        newPage.BackwardNav = GetOneNav(section.SectionUrl, section.SectionUrl, String.Empty);
                    //revolving backwards nav. If there is a root page, it is the backwards nav otherwise we nab the last page in the list
                    else if (parentNode.ChildNodes.Count > 1 && parentNode.ChildNodes[parentNode.ChildNodes.Count - 1].Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                        newPage.BackwardNav = MakeNav(parentNode.ChildNodes[parentNode.ChildNodes.Count - 1], section.SectionUrl);

                    if (page.NextSibling != null && page.NextSibling.Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                        newPage.ForwardNav = MakeNav(page.NextSibling, section.SectionUrl);
                    //forward nav on the last page is the root page
                    else if (!noSectionRootPage)
                        newPage.ForwardNav = GetOneNav(section.SectionUrl, section.SectionUrl, String.Empty);
                    //or it is the first page in the list
                    else if (newPage.Url.Equals(section.SectionUrl + parentNode.ChildNodes[parentNode.ChildNodes.Count - 1].InnerText, StringComparison.InvariantCultureIgnoreCase)
                        && parentNode.ChildNodes.Count > 1 && parentNode.FirstChild.Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                        newPage.ForwardNav = MakeNav(parentNode.FirstChild, section.SectionUrl);
                }

                sectionContentPageList.Add(newPage);
            }

            foreach (XmlNode subSection in subSections)
            {
                var mobileSub = subSection.GetAttribute<bool>("mobile", false);

                if (mobile && !mobileSub)
                    continue;

                sectionContentPageList.AddRange(ReadSubSectionPages(subSection, section, mobile));
            }

            return sectionContentPageList;
        }

        internal IEnumerable<IContentPage> ReadSubSectionPages(XmlNode node, IContentSection section, bool mobile)
        {
            if (node == null)
                return Enumerable.Empty<IContentPage>();

            var noSectionRootPage = node.GetAttribute<bool>("noRootPage", false);
            var subSectionRootPageTitle = node.GetAttribute<String>("rootPageName", String.Empty);
            var subSectionSlideshow = node.GetAttribute<bool>("slideshow", false);
            var subPages = node.SelectNodes("Page");
            var subMeta = node.SelectSingleNode("Meta");
            var subSectionVisible = node.GetAttribute<bool>("visible", true);
            var subSectionSiteMapPriority = node.GetAttribute<float>("siteMapPriority", 1);

            var subSectionUrl = String.Format("{0}{1}", section.SectionUrl, node.GetAttribute<String>("relativeUrl", String.Empty));
            var subSectionName = node.GetAttribute<String>("name", String.Empty);

            var subNav = Enumerable.Empty<INavElement>();

            if (subSectionVisible)
                subNav = GetSubNav(subSectionUrl, subPages
                           , !String.IsNullOrEmpty(subSectionRootPageTitle) ? subSectionRootPageTitle : subSectionName
                           , noSectionRootPage, mobile);

            var subSection = new ContentSubSection
            {
                SubSectionSlideshow = subSectionSlideshow,
                SubSectionRootPageTitle = subSectionRootPageTitle,
                SubSectionMeta = String.Format("{0} {1}", section.SectionMeta, subMeta.InnerText),
                SubSectionName = node.GetAttribute<String>("name", String.Empty),
                SubSectionUrl = subSectionUrl,
                SubNav = GetSubNav(subSectionUrl, subPages
                           , !String.IsNullOrEmpty(subSectionRootPageTitle) ? subSectionRootPageTitle : subSectionName
                           , noSectionRootPage, mobile),
                SectionSlideshow = section.SectionSlideshow,
                SectionRootPageTitle = section.SectionRootPageTitle,
                SectionMeta = section.SectionMeta,
                SectionName = section.SectionName,
                SectionUrl = section.SectionUrl,
                SectionNav = section.SectionNav
            };

            var subPageList = new List<IContentPage>();

            //Add a root page
            if (!noSectionRootPage)
            {
                var rootSectionPage = ReadPageFrom(subSection.SubSectionUrl, subSection.SubSectionUrl, subSection, subSectionSiteMapPriority, mobile, subSectionVisible);

                if (subSectionVisible)
                {
                    if (subPages.Count > 0 && subPages[0].Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                        rootSectionPage.ForwardNav = MakeNav(subPages[0], subSection.SubSectionUrl);

                    //Make revolving backwards nav
                    if (subPages.Count > 0 && subPages[subPages.Count - 1].Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                        rootSectionPage.BackwardNav = MakeNav(subPages[subPages.Count - 1], subSection.SubSectionUrl);
                }


                subPageList.Add(rootSectionPage);
            }

            foreach (XmlNode page in subPages)
            {
                var mobilePage = page.GetAttribute<bool>("mobile", false);

                if (mobile && !mobilePage)
                    continue;

                //invisible means not in the nav
                var visible = page.GetAttribute<bool>("visible", true);

                var contentSwitch = page.GetAttribute<bool>("explicitContent", false);
                var url = subSectionUrl + page.InnerText;
                var pageKey = url;

                if (contentSwitch)
                    pageKey = page.InnerText;

                var pageSiteMapPriority = page.GetAttribute<float>("siteMapPriority", 1);

                var newPage = ReadPageFrom(pageKey, url, subSection, pageSiteMapPriority, mobile, visible);

                //invisible pages dont need to waste our time with nav
                if (visible)
                {
                    var parentNode = page.ParentNode;
                    if (page.PreviousSibling != null && page.PreviousSibling.Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                         newPage.BackwardNav = MakeNav(page.PreviousSibling, subSectionUrl);
                    //This is a first page and we need to make a root nav
                    else if (!noSectionRootPage)
                        newPage.BackwardNav = GetOneNav(subSection.SubSectionUrl, subSection.SubSectionUrl, String.Empty);
                    //revolving backwards nav. If there is a root page, it is the backwards nav otherwise we nab the last page in the list
                    else if (parentNode.ChildNodes.Count > 1 && parentNode.ChildNodes[parentNode.ChildNodes.Count - 1].Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                        newPage.BackwardNav = MakeNav(parentNode.ChildNodes[parentNode.ChildNodes.Count - 1], subSection.SubSectionUrl);

                    if (page.NextSibling != null && page.NextSibling.Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                          newPage.ForwardNav = MakeNav(page.NextSibling, subSectionUrl);
                    //forward nav on the last page is the root page
                    else if (!noSectionRootPage)
                        newPage.ForwardNav = GetOneNav(subSection.SubSectionUrl, subSection.SubSectionUrl, String.Empty);
                    //or it is the first page in the list
                    else if (newPage.Url.Equals(subSection.SubSectionUrl + parentNode.ChildNodes[parentNode.ChildNodes.Count - 1].InnerText, StringComparison.InvariantCultureIgnoreCase)
                        && parentNode.ChildNodes.Count > 1 && parentNode.FirstChild.Name.Equals("Page", StringComparison.InvariantCultureIgnoreCase))
                        newPage.ForwardNav = MakeNav(parentNode.FirstChild, subSection.SubSectionUrl);
                }

                subPageList.Add(newPage);
            }

            return subPageList;
        }

        internal IContentPage ReadPageFrom(String contentKey, String url, IContentSubSection section, float siteMapPriority, bool mobile, bool visible)
        {
            var pageEntry = GetPageEntry(contentKey);

            if (pageEntry == null)
                return null;

            var data = Kernel.Get<IContentPage>();
            data.Backgrounds = pageEntry.Backgrounds;
            data.Meta = String.Format("{0} {1}", section.SubSectionMeta, pageEntry.Meta);
            data.PartialLocation = pageEntry.PartialLocation;
            data.SectionNav = section.SectionNav;
            data.SubNav = section.SubNav;
            data.Title = pageEntry.Title;
            data.InfoText = pageEntry.InfoText;
            data.Url = url.EndsWith("/") ? url : url + "/"; // silly feature of the routing system
            data.SocialMediaFeedItems = pageEntry.SocialMediaFeedItems;
            data.Visible = visible;
            data.SiteMapPriority = siteMapPriority;

            data.SubSectionSlideshow = section.SubSectionSlideshow;
            data.SubSectionRootPageTitle = section.SubSectionRootPageTitle;
            data.SubSectionMeta = section.SubSectionMeta;
            data.SubSectionName = section.SubSectionName;
            data.SubSectionUrl = section.SubSectionUrl;

            data.SectionSlideshow = section.SectionSlideshow;
            data.SectionRootPageTitle = section.SectionRootPageTitle;
            data.SectionMeta = section.SectionMeta;
            data.SectionName = section.SectionName;
            data.SectionUrl = section.SectionUrl;

            return data;
        }

        internal IEnumerable<INavElement> GetSubNav(String baseUrl, XmlNodeList nodes, String sectionName, bool noRootPage, bool mobile)
        {
            var returnList = new List<INavElement>();

            //We need a root node
            if(!noRootPage)
                returnList.Add(GetOneNav(baseUrl, baseUrl, sectionName));

            foreach (XmlNode node in nodes)
            {
                var mobilePage = node.GetAttribute<bool>("mobile", false);

                if (mobile && !mobilePage)
                    continue;

                //invisible means not in the nav
                var visible = node.GetAttribute<bool>("visible", true);

                if (!visible)
                    continue;

                var pageUrl = baseUrl + node.InnerText;
                var navName = node.GetAttribute<String>("name", String.Empty);

                if (String.IsNullOrEmpty(pageUrl))
                    continue;

                //override the inhereted cascading url key structure if we need to
                var contentSwitch = node.GetAttribute<bool>("explicitContent", false);

                var key = pageUrl;
                if (contentSwitch)
                    key = node.InnerText;

                returnList.Add(GetOneNav(key, pageUrl, navName));
            }

            return returnList;
        }

        internal IEnumerable<INavElement> GetSectionNav(String baseUrl, XmlNodeList nodes, String sectionName, bool noRootPage, bool mobile)
        {
            var returnList = new List<INavElement>();

            //We need a root node
            if (!noRootPage)
                returnList.Add(GetOneNav(baseUrl, baseUrl, sectionName));

            foreach (XmlNode node in nodes)
            {
                var mobilePage = node.GetAttribute<bool>("mobile", false);

                if (mobile && !mobilePage)
                    continue;

                //invisible means not in the nav
                var visible = node.GetAttribute<bool>("visible", true);

                if (!visible)
                    continue;

                var pageUrl = baseUrl + node.GetAttribute<String>("relativeUrl", String.Empty);
                var navName = node.GetAttribute<String>("name", String.Empty);

                if (String.IsNullOrEmpty(pageUrl))
                    continue;

                //override the inhereted cascading url key structure if we need to
                var contentSwitch = node.GetAttribute<bool>("explicitContent", false);

                var key = pageUrl;
                if (contentSwitch)
                    key = node.InnerText;

                returnList.Add(GetOneNav(key, pageUrl, navName));
            }

            return returnList;
        }

        internal INavElement GetOneNav(String pathKey, String url, String nameOverride)
        {
            var pageEntry = GetPageEntry(pathKey);

            if (pageEntry == null)
                return null;

            var navElement = Kernel.Get<NavElement>();
            navElement.Name = String.IsNullOrEmpty(nameOverride) ? pageEntry.Title : nameOverride;
            navElement.Url = url.EndsWith("/") ? url : url + "/";
            navElement.Thumbnail = pageEntry.ThumbnailUrl;

            return navElement;
        }

        internal IContentEntry GetPageEntry(String pathKey)
        {
            return ContentEntryRepository.Get(pathKey);
        }

        internal INavElement MakeNav(XmlNode targetPage, String parentUrl)
        {
            var newPage = targetPage;
            var contentSwitch = newPage.GetAttribute<bool>("explicitContent", false);
            var url = parentUrl + newPage.InnerText;
            var pageKey = url;

            if (contentSwitch)
                pageKey = newPage.InnerText;

            return GetOneNav(pageKey, url, String.Empty);
        }
    }
}
