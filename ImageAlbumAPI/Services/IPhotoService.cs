using System.Collections.Generic;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;

namespace ImageAlbumAPI.Services
{
    public interface IPhotoService : IPhotoRepo
    {
        void LikePhoto(Photo photoModel, string UserId);
        void AddComment(Photo photoModel, Comment model);
        void RemoveComment(Photo photoModel, Comment model);
    }
}