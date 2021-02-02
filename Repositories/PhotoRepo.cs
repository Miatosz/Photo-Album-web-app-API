using System.Collections.Generic;
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

        public IEnumerable<Photo> Photos => _context.Photos.Include(c => c.Album);

        public void AddPhoto(Photo photo)
        {
            if (photo.Id == 0)
            {
                _context.Photos.Add(photo);
            }
            _context.SaveChanges();
        }

        public ActionResult DeletePhoto(int id)
        {
            var photo = _context.Photos.Find(id);
            if (photo != null)
            {
                _context.Photos.Remove(photo);
                _context.SaveChanges();
                return new OkResult();
            }
            return new NotFoundResult();
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
                
            }
            _context.SaveChanges();
        }
    }
}