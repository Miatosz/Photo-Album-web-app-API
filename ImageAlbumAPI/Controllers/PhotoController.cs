using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImageAlbumAPI.Dtos.GetDtos;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Models.BindingModels;
using ImageAlbumAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ImageAlbumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IPhotoService _photoService;
        // private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public PhotoController(IPhotoService photoService, IMapper mapper, UserManager<User> userManager)
        {
            _userManager = userManager;
            _photoService = photoService;
            // _userService = userService;
            _mapper = mapper;
        }
        
        // GET: api/photo
        [HttpGet]
        public ActionResult<IEnumerable<GetPhotoDto>> Get()
        {
            var photos = _photoService.Photos;
            var getPhotoDto = _mapper.Map<IEnumerable<GetPhotoDto>>(photos).ToList();

            getPhotoDto.ForEach(c => c.Comments  = new List<GetCommentDto>(
                                _mapper.Map<IEnumerable<GetCommentDto>>(c.Comments))
                            );

            getPhotoDto.ForEach(c => c.Comments
                            .ForEach(d => d.Replies = new List<GetCommentDto>(
                                _mapper.Map<IEnumerable<GetCommentDto>>(d.Replies))
                            ));      
          
            return Ok(getPhotoDto);
        }
        
        // GET: api/photo/{id}
        [HttpGet("{id}")]
        public ActionResult<GetPhotoDto> GetById(int id)
        {
            var photo = _photoService.GetPhotoById(id);
            if (photo == null)
            {
                return NotFound();
            }            
            return Ok(_mapper.Map<GetPhotoDto>(photo));
        }

        // POST: api/photo
        [HttpPost]
        [Authorize]
        public ActionResult<Photo> PostPhoto([FromBody] Photo photo)
        {
            if (ModelState.IsValid)
            {
                photo.Likes = new List<Like>();
                photo.Comments = new List<Comment>();
                _photoService.AddPhoto(photo);
                return Ok(photo);
            }
            return BadRequest();
        }

        // POST: api/photo/{id}/like
        [HttpPost("{id}/like")]
        [Authorize]
        [Route("{id}/like")]
        public ActionResult LikePhoto(int id)
        {
            var photo = _photoService.GetPhotoById(id);
            
            if (photo == null)
            {
                return NotFound();
            }

            var user = GetCurrentLoggedUser().Result;


            if (!photo.Likes.Any(c => c.UserId == user.Id))
            {
                _photoService.LikePhoto(photo, user.Id);
                return Ok();
            }
            else
            {
                return BadRequest("allready liked");
            }                           
        }           


        // POST: api/photo/{id}/unlike
        [HttpPost("{id}/unlike")]
        [Authorize]
        [Route("{id}/unlike")]
        public ActionResult UnlikePhoto(int id)
        {
            var photo = _photoService.GetPhotoById(id);
            if (photo == null)
            {
                return NotFound();
            }

            var user = GetCurrentLoggedUser().Result;

            if (photo.Likes.Any(c => c.UserId == user.Id))
            {
                _photoService.UnlikePhoto(photo, user.Id);
                return Ok();
            }
            else
            {
                return BadRequest("you didn't liked this photo");
            }                   
        }

        

        // POST: api/photo/{id}/comment
        [HttpPost("{id}/comment")]
        [Authorize]
        [Route("{id}/comment")]
        public ActionResult AddComment(int id, Comment model)
        {
            var photo = _photoService.GetPhotoById(id);
            model.User = GetCurrentLoggedUser().Result;
            model.UserId = GetCurrentLoggedUser().Result.Id;

            if (photo != null)
            {
                _photoService.AddComment(photo, model);
                return Ok();
            }
            return NotFound();
        }

        // POST: api/photo/{id}/uncomment
        [HttpPost("{id}/uncomment")]
        [Authorize]
        [Route("{id}/uncomment")]
        public ActionResult RemoveComment(int id, Comment model)
        {
            var photo = _photoService.GetPhotoById(id);
            model.User = GetCurrentLoggedUser().Result;
            model.UserId = GetCurrentLoggedUser().Result.Id;

            if (photo != null)
            {
                if (photo.Comments.Any(c => c.Id == model.Id))
                {
                    _photoService.RemoveComment(photo, model);
                    return Ok();
                }                
            }
            return NotFound();
        }


        // POST: api/photo/{id}/comment/{id}/reply
        [HttpPost("{photoId}/comment/{commentId}/reply")]
        [Route("{photoId}/comment/{commentId}/reply")]
        [Authorize]
        public ActionResult AddReply(int photoId, int commentId, Reply model)
        {
            var photo = _photoService.GetPhotoById(photoId);
            var comment = photo.Comments.FirstOrDefault(c => c.Id == commentId);
            model.User = GetCurrentLoggedUser().Result;
            model.UserId = GetCurrentLoggedUser().Result.Id;
            
            if (comment != null)
            {
                // _photoService.AddComment(photo, comment);
                _photoService.AddReply(comment, model, photo);
                
                return Ok();
            }
            return NotFound();
            // var photo = _photoService.GetPhotoById(photoId);
            // if (photo != null)
            // {
            //     _photoService.AddComment(photo, model);
            //     return Ok();
            // }
            // return NotFound();
        }


        // DELETE: api/photo/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public void DeletePhoto(int id)
        {
            var photo = _photoService.GetPhotoById(id);
            var user = GetCurrentLoggedUser().Result;
            // user.Photos = _photoService.Photos.Where(c => c.Album.UserId == user.Id).ToList();
            
            if (user.Photos.Contains(photo))
            {
                _photoService.DeletePhoto(id);
            }
        }


        // PUT: api/photo
        [HttpPut]
        [Authorize]
        public ActionResult<GetPhotoDto> PutPhoto([FromBody] Photo photo)
        {
            var user = GetCurrentLoggedUser().Result;
            

            if (user.Photos.FirstOrDefault(c => c.Id == photo.Id) != null)
            {
                _photoService.UpdatePhoto(photo);
            }            
            return Ok(_mapper.Map<GetPhotoDto>(photo));
        }

        // PATCH: api/photo/{id}
        [HttpPatch("{id}")]
        [Authorize]
        public StatusCodeResult PatchPhoto(int id, [FromBody] JsonPatchDocument<Photo> patch)
        {
            var user = GetCurrentLoggedUser().Result;
            var photo = _photoService.GetPhotoById(id);
            
            if (user.Photos.Contains(photo))
            {
                if (photo != null)
                {
                    patch.ApplyTo(photo);
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();            
        }    

        public async Task<User> GetCurrentLoggedUser()
        {   
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            user.Photos = _photoService.Photos.Where(c => c.Album.UserId == user.Id).ToList();

            return user;
        }
            
    }
}