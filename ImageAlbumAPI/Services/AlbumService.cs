using System.Collections.Generic;
using System.Linq;
using ImageAlbumAPI.Data;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImageAlbumAPI.Services
{
    public class AlbumService : AlbumRepo, IAlbumService
    {
        private readonly IAlbumRepo _albumRepo;
        private readonly IPhotoRepo _photoRepo;

        public AlbumService(AppDbContext ctx, IAlbumRepo albumRepo, IPhotoRepo photoRepo) : base(ctx)
        {
            _albumRepo = albumRepo;
            _photoRepo = photoRepo;
        }

        public Album GetAlbumById(int id)
            => _albumRepo.Albums.FirstOrDefault(c => c.Id == id);

        public IEnumerable<Photo> GetAlbumPhotos(int albumId)
        {
            IEnumerable<Photo> albumPhotos = _photoRepo.Photos.Where(c => c.AlbumId == albumId);
            return albumPhotos;
        }

        public IEnumerable<Album> GetAlbums()
            => _albumRepo.Albums;
        
    }
}