using System;

namespace Site.Data.API
{
    public interface IFacebookPost
    {
        string Title { get; set; }
        string Url { get; set; }
        string ID { get; set; }
        string Description { get; set; }
        DateTime Published { get; set; }

        ISocialMediaItem AsSocialMediaItem();
    }

    public static partial class SMIHelper
    {
        public static ISocialMediaItem ToSocialMediaItem(this IFacebookPost post)
        {
            return post == null ? null : post.AsSocialMediaItem();
        }
    }
}
