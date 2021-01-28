using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageAlbumAPI.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        
        public int? UserId { get; set; } 
        
        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}