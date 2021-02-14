using System.Collections.Generic;
using ImageAlbumAPI.Models;

namespace ImageAlbumAPI.Repositories
{
    public interface IAlbumRepo
    {
        IEnumerable<Album> Albums {get;}
        void AddAlbum(Album album);
        void DeleteAlbum(int id);
        void UpdateAlbum(Album album);
    }
}