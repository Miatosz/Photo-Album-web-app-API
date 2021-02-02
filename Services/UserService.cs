using System.Collections.Generic;
using System.Linq;
using ImageAlbumAPI.Data;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;

namespace ImageAlbumAPI.Services
{
    public class UserService : UserRepo, IUserService 
    {
        private readonly IUserRepo _userRepo;
        private readonly IPhotoRepo _photoRepo;
        private readonly IAlbumRepo _albumRepo;

        public UserService(AppDbContext ctx, IUserRepo userRepo, IPhotoRepo photoRepo, IAlbumRepo albumRepo) : base(ctx)
        {
            _userRepo = userRepo;
            _photoRepo = photoRepo;
            _albumRepo = albumRepo;
        }
        public IEnumerable<User> GetUsers()
            => _userRepo.Users;
        public User GetUserById(int id)
        {
            var user = _userRepo.Users.FirstOrDefault(c => c.Id == id);
            if (user != null)
            {
                return user;
            }
            return null;
        }  

        public IEnumerable<Album> GetUserAlbums(int userId)
        {
            var albums = _albumRepo.Albums.Where(c => c.UserId == userId);
            return albums;
        }


        public IEnumerable<Photo> GetUserPhotos(int userId)
        {
            var photos = _photoRepo.Photos.Where(c => c.Album.UserId == userId);
            return photos;
        }

    }
}