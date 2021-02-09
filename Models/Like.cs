using System.ComponentModel.DataAnnotations.Schema;

namespace ImageAlbumAPI.Models
{
    public class Like
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string UserId { get; set; } 
        
        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}