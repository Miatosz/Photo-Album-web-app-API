using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImageAlbumAPI.Dtos.GetDtos;
using ImageAlbumAPI.Models;
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
        private readonly IMapper _mapper;

        public PhotoController(IPhotoService photoService, IMapper mapper)
        {
            _photoService = photoService;
            _mapper = mapper;
        }
        
        // GET: api/photo
        [HttpGet]
        public ActionResult<List<GetPhotoDto>> Get()
            => _mapper.Map<IEnumerable<GetPhotoDto>>(_photoService.GetPhotos()).ToList();
          

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
                _photoService.AddPhoto(photo);
                return Ok(photo);
            }
            return BadRequest();
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