using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Site.Data.API
{
    public interface IContentSection
    {
        bool SectionSlideshow { get; set; }
        string SectionRootPageTitle { get; set; }
        string SectionName { get; set; }
        string SectionUrl { get; set; }
        string SectionMeta { get; set; }

        IEnumerable<INavElement> SectionNav { get; set; }
    }

    public interface IContentSubSection : IContentSection
    {
        bool SubSectionSlideshow { get; set; }
        string SubSectionRootPageTitle { get; set; }
        string SubSectionName { get; set; }
        string SubSectionUrl { get; set; }
        string SubSectionMeta { get; set; }

        IEnumerable<INavElement> SubNav { get; set; }
    }

    public interface IContentPage : IContentSubSection
    {
        string Url { get; set; }
        string Title { get; set; }
        string Meta { get; set; }
        string PartialLocation { get; set; }
        bool Visible { get; set; }
        string InfoText { get; set; }
        float SiteMapPriority { get; set; }

        IEnumerable<string> Backgrounds { get; set; }
        IEnumerable<INavElement> PrimaryNav { get; set; }
        INavElement ForwardNav { get; set; }
        INavElement BackwardNav { get; set; }
        IEnumerable<ISocialMediaItem> SocialMediaFeedItems { get; set; }
    }

    public interface IContentEntry
    {
        string Key { get; set; }
        string Title { get; set; }
        string Meta { get; set; }
        string PartialLocation { get; set; }
        string ThumbnailUrl { get; set; }
        string InfoText { get; set; }
        IEnumerable<ISocialMediaItem> SocialMediaFeedItems { get; set; }
        IEnumerable<string> Backgrounds { get; set; }
    }
}
