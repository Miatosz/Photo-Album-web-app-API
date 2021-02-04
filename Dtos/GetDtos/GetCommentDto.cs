using System.Collections.Generic;

namespace ImageAlbumAPI.Dtos.GetDtos
{
    public class GetCommentDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }

        public List<GetCommentDto> Replies { get; set; }
    }
}