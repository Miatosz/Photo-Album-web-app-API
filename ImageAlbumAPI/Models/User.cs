using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ImageAlbumAPI.Models
{
    public class User : IdentityUser
    {
        
        public int UserId { get; set; }
        public override string UserName { get; set; }

        public virtual ICollection<Photo> Photos {get; set;}
        public virtual ICollection<Album> Albums {get; set;}
        public virtual ICollection<Like> Likes {get; set;}
        public virtual ICollection<Comment> Comments {get; set;}


        public virtual ICollection<User> Following {get; set;}
        public virtual ICollection<User> Followers {get; set;}


    }
}