using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImageAlbumAPI.Data;
using ImageAlbumAPI.Dtos.GetDtos;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImageAlbumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPhotoRepo _repo;
        private readonly IMapper _mapper;

        public PhotoController(IPhotoRepo repo, IMapper mapper, AppDbContext ctx)
        {
            _context = ctx;
            _repo = repo;
            _mapper = mapper;
        }

        // GET: api/photo
        [HttpGet]
        public ActionResult<IEnumerable<GetPhotoDto>> Get()
            => Ok(_mapper.Map<IEnumerable<GetPhotoDto>>(_repo.Photos));

        // GET: api/photo/{id}
        [HttpGet("{id}")]
        public ActionResult<GetPhotoDto> Get(int id)
        {
            var photo = _context.Photos.Include(c => c.Album).FirstOrDefault(c => c.Id == id);
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
            _repo.AddPhoto(photo);
            return Ok();
        }

        // DELETE: api/photo/{id}
        [HttpDelete("{id}")]
        public void DeletePhoto(int id)
        {
            _repo.DeletePhoto(id);
        }

        // PUT: api/photo
        [HttpPut]
        public ActionResult<GetPhotoDto> PutPhoto([FromBody] Photo photo)
        {
            _repo.UpdatePhoto(photo);
            return Ok(_mapper.Map<GetPhotoDto>(photo));
        }

        // PATCH: api/photo/{id}
        [HttpPatch("{id}")]
        public StatusCodeResult PatchPhoto(int id, [FromBody] JsonPatchDocument<Photo> patch)
        {
            var photo = _repo.Photos.First(c => c.Id == id);
            if (photo != null)
            {
                patch.ApplyTo(photo);
                return Ok();
            }
            return NotFound();
        }
    
    }
}