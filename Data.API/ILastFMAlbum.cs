using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Site.Data.API
{
    public interface ILastFMAlbum
    {
        string Name { get; set; }
        string ArtistName { get; set; }
        String Url { get; set; }
        int PlayCount { get; set; }

        ISocialMediaItem AsSocialMediaItem();
    }

    public static partial class SMIHelper
    {
        public static ISocialMediaItem ToSocialMediaItem(this ILastFMAlbum album)
        {
            return album == null ? null : album.AsSocialMediaItem();
        }
    }

    /*
      <album rank="1">
        <artist mbid="9ddce51c-2b75-4b3e-ac8c-1db09e7c89c6">Burial</artist>
        <name>Untrue</name>
        <mbid/>
        <playcount>35</playcount>
        <url>http://www.last.fm/music/Burial/Untrue</url>
      </album>
     */
}
