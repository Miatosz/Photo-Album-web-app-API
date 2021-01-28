using System.Collections.Generic;
using ImageAlbumAPI.Models;

namespace ImageAlbumAPI.Repositories
{
    public interface IPhotoRepo
    {
        IEnumerable<Photo> Photos {get;}
        void AddPhoto(Photo photo);
        Photo DeletePhoto(int id);
        void UpdatePhoto(Photo photo);
    }
}