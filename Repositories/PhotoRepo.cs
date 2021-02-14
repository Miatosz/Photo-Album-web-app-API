using System.Collections.Generic;
using System.Linq;
using ImageAlbumAPI.Data;
using ImageAlbumAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImageAlbumAPI.Repositories
{
    public class PhotoRepo : IPhotoRepo
    {
        private readonly AppDbContext _context;

        public PhotoRepo(AppDbContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Photo> Photos => _context.Photos.Include(c => c.Album)
                                                            .Include(c => c.Likes)
                                                            .Include(c => c.Comments)
                                                            .ThenInclude(c => c.Replies);

        public void AddPhoto(Photo photo)
        {
            if (photo.Id == 0)
            {
                _context.Photos.Add(photo);
            }
            _context.SaveChanges();
        }

        public void DeletePhoto(int id)
        {
            var photo = _context.Photos.Find(id);
            if (photo == null)
            {
                return;
            }
            _context.Photos.Remove(photo);
            _context.SaveChanges();            
        }

        public void UpdatePhoto(Photo photo)
        {
            var updatedPhoto = _context.Photos.Find(photo.Id);
            if (updatedPhoto != null)
            {
                updatedPhoto.Description = photo.Description;
                updatedPhoto.DateOfAdd = photo.DateOfAdd;
                updatedPhoto.Album = photo.Album;
                updatedPhoto.PhotoPath = photo.PhotoPath;
                updatedPhoto.Likes = photo.Likes;
                updatedPhoto.NumberOfLikes = photo.NumberOfLikes;
                updatedPhoto.Comments = photo.Comments;
                // updatedPhoto.Comments.ForEach(c => c.Replies = photo.Comments)
                
            }
            _context.SaveChanges();
        }

        public void UpdateComments(Photo photo)
        {
            var updatedPhoto = _context.Photos.Find(photo.Id);
            if (updatedPhoto != null)
            { 
                updatedPhoto.Comments = photo.Comments;
            }
            _context.SaveChanges();
        }

        public Photo GetPhotoById(int id)
        {
            var photo = this.Photos?.ToList().FirstOrDefault(c => c.Id == id);
            if (photo != null)
            {
                return photo;
            }
            return null; 
        }
       

        public void UnlikePhoto(Photo photoModel, string UserId)
        {
            photoModel.NumberOfLikes--;
            photoModel.Likes.RemoveAll(c => c.UserId == UserId);
            UpdatePhoto(photoModel);   
        }



        public void AddReply(Comment comment, Reply reply, Photo photo)
        {
            if (comment.Replies == null)
            {
                comment.Replies = new List<Reply>();
            }
            comment.Replies.Add(reply);
            UpdateComments(photo);
        }

        public void RemoveReply(Comment comment, Reply reply)
        {
            throw new System.NotImplementedException();
        }
    }
}