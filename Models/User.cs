using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageAlbumAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }

        
    }
}