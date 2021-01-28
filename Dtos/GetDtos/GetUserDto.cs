using System.Collections.Generic;
using ImageAlbumAPI.Models;

namespace ImageAlbumAPI.Dtos.GetDtos
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<GetAlbumDto> Albums { get; set; }
    }
}