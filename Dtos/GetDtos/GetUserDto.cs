using System.Collections.Generic;
using ImageAlbumAPI.Models;

namespace ImageAlbumAPI.Dtos.GetDtos
{
    public class GetUserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<GetAlbumDto> Albums { get; set; }
        public List<GetUserDto> Following { get; set; }
        public List<GetUserDto> Followers { get; set; }

        
    }
}