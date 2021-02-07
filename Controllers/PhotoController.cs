using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImageAlbumAPI.Dtos.GetDtos;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Models.BindingModels;
using ImageAlbumAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ImageAlbumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public PhotoController(IPhotoService photoService, IMapper mapper, IUserService userService)
        {
            _photoService = photoService;
            _userService = userService;
            _mapper = mapper;
        }
        
        // GET: api/photo
        [HttpGet]
        public ActionResult<Photo> Get()
        {
            var photos = _photoService.GetPhotos();
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
            //10/comment/1/reply
          

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
        public ActionResult PostPhoto([FromBody] Photo photo)
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
        [Route("{id}/like")]
        public ActionResult LikePhoto(int id, [FromBody] Like model)
        {
            var photo = _photoService.GetPhotoById(id);
            
            if (photo != null)
            {
                // if (photo.Likes == null)
                // {
                //     photo.Likes = new List<Like>();
                // }
                if (!photo.Likes.Any(c => c.UserId == model.UserId))
                {
                    _photoService.LikePhoto(photo, model);
                    return Ok();
                }
                else
                {
                    return BadRequest("allready liked");
                }                           
            }           
            return NotFound();            
        }

        // POST: api/photo/{id}/unlike
        [HttpPost("{id}/unlike")]
        [Route("{id}/unlike")]
        public ActionResult UnlikePhoto(int id, [FromBody] Like model)
        {
            var photo = _photoService.GetPhotoById(id);
            if (photo != null)
            {
                if (photo.Likes.Any(c => c.UserId == model.UserId))
                {
                    _photoService.UnlikePhoto(photo, model);
                    return Ok();
                }
                else
                {
                    return BadRequest("you didn't liked this photo");
                }                           
            }           
            return NotFound();            
        }

        

        // POST: api/photo/{id}/comment
        [HttpPost("{id}/comment")]
        [Route("{id}/comment")]
        public ActionResult AddComment(int id, Comment model)
        {
            var photo = _photoService.GetPhotoById(id);
            if (photo != null)
            {
                _photoService.AddComment(photo, model);
                return Ok();
            }
            return NotFound();
        }

        // POST: api/photo/{id}/uncomment
        [HttpPost("{id}/uncomment")]
        [Route("{id}/uncomment")]
        public ActionResult RemoveComment(int id, Comment model)
        {
            var photo = _photoService.GetPhotoById(id);
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
        public ActionResult AddReply(int photoId, int commentId, Reply model)
        {
            var photo = _photoService.GetPhotoById(photoId);
            var comment = photo.Comments.FirstOrDefault(c => c.Id == commentId);
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
        public ActionResult DeletePhoto(int id)
            => _photoService.DeletePhoto(id);


        // PUT: api/photo
        [HttpPut]
        public ActionResult<GetPhotoDto> PutPhoto([FromBody] Photo photo)
        {
            _photoService.UpdatePhoto(photo);
            return Ok(_mapper.Map<GetPhotoDto>(photo));
        }

        // PATCH: api/photo/{id}
        [HttpPatch("{id}")]
        public StatusCodeResult PatchPhoto(int id, [FromBody] JsonPatchDocument<Photo> patch)
        {
            var photo = _photoService.GetPhotoById(id);
            if (photo != null)
            {
                patch.ApplyTo(photo);
                return Ok();
            }
            return NotFound();
        }    
    }
}