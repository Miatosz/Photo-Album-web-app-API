using System.Collections.Generic;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;

namespace ImageAlbumAPI.Services
{
    public interface IPhotoService : IPhotoRepo
    {
        IEnumerable<Photo> GetPhotos();
        Photo GetPhotoById(int id);

    }
}