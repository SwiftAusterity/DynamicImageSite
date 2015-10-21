using System;
using System.Collections.Generic;

namespace Site.Data.API.Repository
{
    public interface ILastFMRepository
    {
        IEnumerable<ILastFMTrack> GetLatestTracks(string username);
        IEnumerable<ILastFMAlbum> GetLatestAlbums(string groupName);
    }

    public interface ILastFMRepositoryBackingStore : ILastFMRepository
    {
        IEnumerable<ILastFMTrack> GetLatestTracks(string username, bool initial);
        IEnumerable<ILastFMAlbum> GetLatestAlbums(string groupName, bool initial);
    }
}
