using System.Collections.Generic;
using ImageAlbumAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageAlbumAPI.Repositories
{
    public interface IPhotoRepo
    {
        IEnumerable<Photo> Photos {get;}
        Photo GetPhotoById(int id);
        void AddPhoto(Photo photo);
        void DeletePhoto(int id);
        void UpdatePhoto(Photo photo);
        void UpdateComments(Photo photo);        
        void UnlikePhoto(Photo photoModel, string UserId);
        void AddReply(Comment comment, Reply reply, Photo photo);
        void RemoveReply(Comment comment, Reply reply);
    }
}