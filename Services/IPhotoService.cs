using System.Collections.Generic;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;

namespace ImageAlbumAPI.Services
{
    public interface IPhotoService : IPhotoRepo
    {
        IEnumerable<Photo> GetPhotos();
        Photo GetPhotoById(int id);
        void LikePhoto(Photo photoModel, string UserId);
        void UnlikePhoto(Photo photoModel, string UserId);
        void AddComment(Photo photoModel, Comment model);
        void RemoveComment(Photo photoModel, Comment model);
        void AddReply(Comment comment, Reply reply, Photo photo);
        void RemoveReply(Comment comment, Reply reply);

    }
}