using System.Collections.Generic;
using ImageAlbumAPI.Data;
using ImageAlbumAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageAlbumAPI.Repositories
{
    public class AlbumRepo : IAlbumRepo
    {
        private readonly AppDbContext _context;

        public AlbumRepo(AppDbContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Album> Albums => _context.Albums.Include(c => c.User);

        public void AddAlbum(Album album)
        {
            if (album.Id == 0)
            {
                _context.Add(album);
            }
            _context.SaveChanges();
        }

        public Album DeleteAlbum(int id)
        {
            var album = _context.Albums.Find(id);
            if (album != null)
            {
                _context.Remove(album);
                _context.SaveChanges();
            }
            return album;
        }

        public void UpdateAlbum(Album album)
        {
            var updatedAlbum = _context.Albums.Find(album.Id);
            if (updatedAlbum != null)
            {
                updatedAlbum.Name = album.Name;
                updatedAlbum.User = album.User;
                updatedAlbum.UserId = album.UserId;
                updatedAlbum.Description = album.Description;
               
            }
            _context.SaveChanges();        
        }
    }
}