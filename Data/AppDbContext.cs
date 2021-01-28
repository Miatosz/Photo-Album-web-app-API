using ImageAlbumAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageAlbumAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

        public DbSet<Album> Albums {get; set;}
        public DbSet<User> Users {get; set;}
        public DbSet<Photo> Photos {get; set;}
    }
}