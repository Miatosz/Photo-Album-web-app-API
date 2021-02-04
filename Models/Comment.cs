using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageAlbumAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }

        public List<Like> Likes {get; set;}
        public List<Comment> Replies {get; set;}

        public int UserId { get; set; } 
        
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int PhotoId { get; set; } 
        
        [ForeignKey("PhotoId")]
        public Photo Photo { get; set; }


    }
}