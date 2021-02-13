using System.Collections.Generic;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;

namespace ImageAlbumAPI.Services
{
    public interface IAlbumService : IAlbumRepo
    {
        IEnumerable<Album> GetAlbums();
        Album GetAlbumById(int id);
        IEnumerable<Photo> GetAlbumPhotos(int albumId);
    }
}