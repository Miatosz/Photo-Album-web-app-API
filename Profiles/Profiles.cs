using AutoMapper;
using ImageAlbumAPI.Dtos.GetDtos;
using ImageAlbumAPI.Models;

namespace ImageAlbumAPI.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            // GET Dtos
            CreateMap<User, GetUserDto>();
            CreateMap<Album, GetAlbumDto>();
            CreateMap<Photo, GetPhotoDto>();
        }
        
    }
}