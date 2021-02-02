using System.Collections.Generic;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;

namespace ImageAlbumAPI.Services
{
    public interface IUserService : IUserRepo
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int id);
        IEnumerable<Album> GetUserAlbums(int userId);
        IEnumerable<Photo> GetUserPhotos(int userId);
    }
}