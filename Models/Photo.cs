using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageAlbumAPI.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public string DateOfAdd { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }

        public int? AlbumId { get; set; } 
        
        [ForeignKey("AlbumId")]
        public Album Album { get; set; }
    }
}