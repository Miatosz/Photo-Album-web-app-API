using System.ComponentModel.DataAnnotations;

namespace ImageAlbumAPI.Models.BindingModels
{
    public class ChangePasswordModel
    {
        //public string Id { get; set; }
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Email { get; set; }
    }
}