using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Site.Data.API
{
    public interface ILastFMTrack
    {
        string Name { get; set; }
        bool NowPlaying { get; set; }
        string ArtistName { get; set; }
        string AlbumName { get; set; }
        string Url { get; set; }
        DateTime Date { get; set; }
        int Streamable { get; set; }

        ISocialMediaItem AsSocialMediaItem();
    }

    public static partial class SMIHelper
    {
        public static ISocialMediaItem ToSocialMediaItem(this ILastFMTrack track)
        {
            return track == null ? null : track.AsSocialMediaItem();
        }
    }
    /*
        <track nowplaying="true">
            <artist mbid="2f9ecbed-27be-40e6-abca-6de49d50299e">Aretha Franklin</artist>
            <name>Sisters Are Doing It For Themselves</name>
            <mbid/>
            <album mbid=""/>
            <url>www.last.fm/music/Aretha+Franklin/_/Sisters+Are+Doing+It+For+Themselves</url>
            <date uts="1213031819">9 Jun 2008, 17:16</date>
            <streamable>1</streamable>
        </track>
     */
}
