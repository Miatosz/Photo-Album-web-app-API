using System.Collections.Generic;
using ImageAlbumAPI.Models;

namespace ImageAlbumAPI.Repositories
{
    public interface IUserRepo
    {
        IEnumerable<User> Users {get;}
        void AddUser(User user);
        User DeleteUser(int id);
        void UpdateUser(User user);
    }
}