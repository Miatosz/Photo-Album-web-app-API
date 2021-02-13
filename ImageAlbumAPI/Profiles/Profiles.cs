using AutoMapper;
using ImageAlbumAPI.Dtos.GetDtos;
using ImageAlbumAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ImageAlbumAPI.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            // GET Dtos
            CreateMap<User, GetUserDto>();
            CreateMap<IdentityUser, GetUserDto>();
            CreateMap<Album, GetAlbumDto>();
            CreateMap<Photo, GetPhotoDto>();
            CreateMap<GetPhotoDto, Photo>();
            CreateMap<Like, GetLikeDto>();
            CreateMap<Comment, GetCommentDto>();
        }
        
    }
}