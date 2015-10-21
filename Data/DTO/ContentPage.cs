using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Site.Data.API;

namespace Site.Data.DTO
{
    [Serializable]
    public class ContentSection : IContentSection
    {
        public bool SectionSlideshow { get; set; }
        public string SectionRootPageTitle { get; set; }
        public string SectionName { get; set; }
        public string SectionUrl { get; set; }
        public string SectionMeta { get; set; }

        public IEnumerable<INavElement> SectionNav { get; set; }
    }

    [Serializable]
    public class ContentSubSection : IContentSubSection
    {
        public bool SubSectionSlideshow { get; set; }
        public string SubSectionRootPageTitle { get; set; }
        public string SubSectionName { get; set; }
        public string SubSectionUrl { get; set; }
        public string SubSectionMeta { get; set; }

        public bool SectionSlideshow { get; set; }
        public string SectionRootPageTitle { get; set; }
        public string SectionName { get; set; }
        public string SectionUrl { get; set; }
        public string SectionMeta { get; set; }

        public IEnumerable<INavElement> SectionNav { get; set; }
        public IEnumerable<INavElement> SubNav { get; set; }
    }

    [Serializable]
    public class ContentPage : IContentPage
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Meta { get; set; }
        public string PartialLocation { get; set; }
        public bool Visible { get; set; }
        public string InfoText { get; set; }
        public float SiteMapPriority { get; set; }

        public IEnumerable<string> Backgrounds { get; set; }
        public INavElement ForwardNav { get; set; }
        public INavElement BackwardNav { get; set; }
        public IEnumerable<INavElement> PrimaryNav { get; set; }
        public IEnumerable<ISocialMediaItem> SocialMediaFeedItems { get; set; }

        public bool SubSectionSlideshow { get; set; }
        public string SubSectionRootPageTitle { get; set; }
        public string SubSectionName { get; set; }
        public string SubSectionUrl { get; set; }
        public string SubSectionMeta { get; set; }

        public bool SectionSlideshow { get; set; }
        public string SectionRootPageTitle { get; set; }
        public string SectionName { get; set; }
        public string SectionUrl { get; set; }
        public string SectionMeta { get; set; }

        public IEnumerable<INavElement> SectionNav { get; set; }
        public IEnumerable<INavElement> SubNav { get; set; }
    }

    [Serializable]
    public class ContentEntry : IContentEntry
    {
        #region IContentEntry Members

        public string Key { get; set; }
        public string Title { get; set; }
        public string Meta { get; set; }
        public string PartialLocation { get; set; }
        public string ThumbnailUrl { get; set; }
        public string InfoText { get; set; }
        public IEnumerable<ISocialMediaItem> SocialMediaFeedItems { get; set; }
        public IEnumerable<string> Backgrounds { get; set; }

        #endregion
    }
}
