using System;
using System.Collections.Generic;

namespace Site.Data.API.Repository
{
    public interface ILastFMRepository
    {
        IEnumerable<ILastFMTrack> GetLatestTracks(String username);
        IEnumerable<ILastFMAlbum> GetLatestAlbums(String groupName);
    }

    public interface ILastFMRepositoryBackingStore : ILastFMRepository
    {
        IEnumerable<ILastFMTrack> GetLatestTracks(String username, bool initial);
        IEnumerable<ILastFMAlbum> GetLatestAlbums(String groupName, bool initial);
    }
}
