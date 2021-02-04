using System.Collections.Generic;
using ImageAlbumAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageAlbumAPI.Repositories
{
    public interface IPhotoRepo
    {
        IEnumerable<Photo> Photos {get;}
        void AddPhoto(Photo photo);
        ActionResult DeletePhoto(int id);
        void UpdatePhoto(Photo photo);
        void UpdateComments(Photo photo);
    }
}