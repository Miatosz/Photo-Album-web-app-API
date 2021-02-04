using System.Collections.Generic;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;

namespace ImageAlbumAPI.Services
{
    public interface IPhotoService : IPhotoRepo
    {
        IEnumerable<Photo> GetPhotos();
        Photo GetPhotoById(int id);
        void LikePhoto(Photo photoModel, Like likeModel);
        void UnlikePhoto(Photo photoModel, Like likeModel);
        void AddComment(Photo photoModel, Comment model);
        void RemoveComment(Photo photoModel, Comment model);
        void AddReply(Comment comment, Comment reply);
        void RemoveReply(Comment comment, Comment reply);

    }
}