using ImageAlbumAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ImageAlbumAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

        public virtual DbSet<Album> Albums {get; set;}
        public virtual DbSet<User> Users {get; set;}
        public virtual DbSet<Photo> Photos {get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>()
                .HasOne<Album>(e => e.Album)
                .WithMany(d => d.Photos)
                .HasForeignKey(e => e.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Album>()
                .HasOne<User>(e => e.User)
                .WithMany(d => d.Albums)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
           
                
        }   
    
    }
}