using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Site.Data.API;

namespace Site.Models
{
    public class ContentPageJson
    {
        public ContentPageJson(IContentPage contentPage, string htmlString)
        {
           html = htmlString;
           title = contentPage.Title;
           meta = contentPage.Meta;
           backgrounds = contentPage.Backgrounds;
           subNav = contentPage.SubNav;
           sectionNav = contentPage.SectionNav;
           forwardNav = contentPage.ForwardNav;
           backwardNav = contentPage.BackwardNav;
           url = contentPage.Url;
           area = contentPage.SectionName;
           section = contentPage.SubSectionName;
        }

        public string html { get; set; } //= RenderHtml(contentPage.PartialLocation, viewModel),
        public string title { get; set; } //= contentPage.Title,
        public string meta { get; set; } //= contentPage.Meta,
        public IEnumerable<string> backgrounds { get; set; } //= contentPage.Backgrounds,
        public IEnumerable<INavElement> subNav { get; set; } //= contentPage.SubNav,
        public IEnumerable<INavElement> sectionNav { get; set; } //= contentPage.SectionNav,
        public INavElement forwardNav { get; set; } //= contentPage.ForwardNav,
        public INavElement backwardNav { get; set; } //= contentPage.BackwardNav,
        public string url { get; set; } //= contentPage.Url,
        public string area { get; set; } //= contentPage.SectionName,
        public string section { get; set; } //= contentPage.SubSectionName,
        public string direction { get; set; } //= direction
    }
}
