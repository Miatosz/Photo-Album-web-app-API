using System.Collections.Generic;
using System.Linq;
using ImageAlbumAPI.Data;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;

namespace ImageAlbumAPI.Services
{
    public class PhotoService : PhotoRepo, IPhotoService
    {
        private readonly IUserRepo _userRepo;
        private readonly IPhotoRepo _photoRepo;
        private readonly IAlbumRepo _albumRepo;


        public PhotoService(AppDbContext ctx, IPhotoRepo photoRepo, IAlbumRepo albumRepo, IUserRepo userRepo) : base(ctx)
        {
            _userRepo = userRepo;
            _photoRepo = photoRepo;
            _albumRepo = albumRepo;
        }

        

        public Photo GetPhotoById(int id)
        {
            var photo = _photoRepo.Photos?.FirstOrDefault(c => c.Id == id);
            if (photo != null)
            {
                return photo;
            }
            return null; 
        }

        public IEnumerable<Photo> GetPhotos()
        {
            return _photoRepo.Photos;
        }

        public void LikePhoto(Photo photoModel, Like model)
        {
            photoModel.NumberOfLikes++;
            model.UserName = _userRepo.Users.FirstOrDefault(c => c.UserId == model.UserId).UserName;
            photoModel.Likes.Add(model);
            _photoRepo.UpdatePhoto(photoModel);           
        }


        public void UnlikePhoto(Photo photoModel, Like model)
        {
            photoModel.NumberOfLikes--;
            photoModel.Likes.RemoveAll(c => c.UserId == model.UserId);
            _photoRepo.UpdatePhoto(photoModel);          
        }

        public void AddComment(Photo photoModel, Comment model)
        {
            model.UserName = _userRepo.Users.FirstOrDefault(c => c.UserId == model.UserId).UserName;
            model.User = _userRepo.Users.FirstOrDefault(c => c.UserId == model.UserId);
            model.Likes = new List<Like>();
            model.Replies = new List<Comment>();
            if (photoModel.Comments == null)
            {
                photoModel.Comments = new List<Comment>();
            }
            photoModel.Comments.Add(model);
            _photoRepo.UpdatePhoto(photoModel);
        }
        public void RemoveComment(Photo photoModel, Comment model)
        {
            model.UserName = _userRepo.Users.FirstOrDefault(c => c.UserId == model.UserId).UserName;
            photoModel.Comments.RemoveAll(c => c.UserId == model.UserId);
            _photoRepo.UpdatePhoto(photoModel);
        }

        public void AddReply(Comment comment, Comment reply)
        {
            var photo = GetPhotoById(comment.PhotoId);
            if (comment.Replies == null)
            {
                comment.Replies = new List<Comment>();
            }
            comment.Replies.Add(reply);
            _photoRepo.UpdateComments(photo);
            //_commentRepo.UpdateComment(comment);           
        }

        public void RemoveReply(Comment comment, Comment reply)
        {
            throw new System.NotImplementedException();
        }
    }
}