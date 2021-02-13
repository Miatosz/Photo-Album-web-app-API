using System.Collections.Generic;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;

namespace ImageAlbumAPI.Services
{
    public interface IUserService : IUserRepo
    {
        IEnumerable<User> GetUsers();
        User GetUserById(string id);
        IEnumerable<Album> GetUserAlbums(string Id);
        IEnumerable<Photo> GetUserPhotos(string Id);
    }
}