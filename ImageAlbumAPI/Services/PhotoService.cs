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



        public PhotoService(AppDbContext ctx, IPhotoRepo photoRepo,  IUserRepo userRepo) : base(ctx)
        {
            _userRepo = userRepo;
            _photoRepo = photoRepo;

        }
        public void LikePhoto(Photo photoModel, string UserId)
        {
            photoModel.NumberOfLikes++;
            photoModel.Likes.Add(new Like
            {
                UserId = UserId,
                UserName = _userRepo.Users.FirstOrDefault(c => c.Id == UserId).UserName               
            });
            _photoRepo.UpdatePhoto(photoModel);           
        }

        public void AddComment(Photo photoModel, Comment model)
        {
            model.UserName = _userRepo.Users.FirstOrDefault(c => c.UserId.ToString() == model.UserId).UserName;
            model.User = _userRepo.Users.FirstOrDefault(c => c.UserId.ToString() == model.UserId);
            model.Likes = new List<Like>();
            model.Replies = new List<Reply>();
            if (photoModel.Comments == null)
            {
                photoModel.Comments = new List<Comment>();
            }
            photoModel.Comments.Add(model);
            _photoRepo.UpdatePhoto(photoModel);
        }
        public void RemoveComment(Photo photoModel, Comment model)
        {
            model.UserName = _userRepo.Users.FirstOrDefault(c => c.UserId.ToString() == model.UserId).UserName;
            photoModel.Comments.RemoveAll(c => c.UserId == model.UserId);
            _photoRepo.UpdatePhoto(photoModel);
        }

    }
}