using System.Collections.Generic;
using ImageAlbumAPI.Data;
using ImageAlbumAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageAlbumAPI.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<User> Users => _context.Users;

        public void AddUser(User user)
        {
            if (user.Id == 0)
            {
                _context.Users.Add(user);
            }
            _context.SaveChanges();
        }

        public User DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return user;
        }

        public void UpdateUser(User user)
        {
            var updatedUser = _context.Users.Find(user.Id);
            if (updatedUser != null)
            {
                updatedUser.UserName = user.UserName;
            }
            _context.SaveChanges();
        }
    }
}