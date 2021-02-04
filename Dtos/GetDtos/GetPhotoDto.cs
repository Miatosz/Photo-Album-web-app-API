using System.Collections.Generic;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Models.BindingModels;

namespace ImageAlbumAPI.Dtos.GetDtos
{
    public class GetPhotoDto
    {
        public string DateOfAdd { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public string AlbumName { get; set; }
        public int NumberOfLikes { get; set; }
        public List<GetLikeDto> Likes { get; set; }
        public List<GetCommentDto> Comments { get; set; }
    }
}