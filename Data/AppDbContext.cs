using ImageAlbumAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ImageAlbumAPI.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

        public virtual DbSet<Album> Albums {get; set;}
        public virtual DbSet<User> Users {get; set;}
        public virtual DbSet<Photo> Photos {get; set;}
        public virtual DbSet<Comment> Comments {get; set;}
        public virtual DbSet<Like> Likes {get; set;}
        public virtual DbSet<Reply> Replies {get; set;}





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("IdentityUserLogin").HasNoKey();
            // modelBuilder.Entity<IdentityUserRole<string>>().ToTable("IdentityUserRole").HasNoKey();
            // modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("IdentityRoleClaim").HasNoKey();
            // modelBuilder.Entity<IdentityRole<string>>().ToTable("IdentityRole").HasNoKey();
            // modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("IdentityUserClaim").HasNoKey();
            //modelBuilder.Entity<IdentityUser<string>>().ToTable("IdentityUser");
            // modelBuilder.Entity<IdentityUserToken<string>>().ToTable("IdentityUserToken").HasNoKey();


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


            modelBuilder.Entity<Comment>()
                .HasDiscriminator<int>("Type")
                .HasValue<Comment>(0)
                .HasValue<Reply>(1);



                
        }   
    
    }
}