using System.Collections.Generic;
using System.Linq;
using ImageAlbumAPI.Data;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;

namespace ImageAlbumAPI.Services
{
    public class PhotoService : PhotoRepo, IPhotoService
    {
        private readonly IPhotoRepo _photoRepo;
        private readonly IAlbumRepo _albumRepo;

        public PhotoService(AppDbContext ctx, IPhotoRepo photoRepo, IAlbumRepo albumRepo) : base(ctx)
        {
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
    }
}