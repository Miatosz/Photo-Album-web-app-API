using System.Collections.Generic;
using ImageAlbumAPI.Models;

namespace ImageAlbumAPI.Dtos.GetDtos
{
    public class GetAlbumDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerName { get; set; }

        public List<GetPhotoDto> Photos { get; set; }
    }
}