using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ImageAlbumAPI.Models.BindingModels;

namespace ImageAlbumAPI.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }        
        public string DateOfAdd { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public int NumberOfLikes{ get; set; }
        public List<Like> Likes {get; set;}
        public List<Comment> Comments {get; set;}

        public int? AlbumId { get; set; } 
        
        [ForeignKey("AlbumId")]
        public Album Album { get; set; }
    }

    
}