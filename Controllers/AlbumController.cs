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
    public class AlbumController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IAlbumService _albumService;
        private readonly IUserService _userService;

        public AlbumController(IAlbumService albumService, IPhotoService photoService, IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _photoService = photoService;
            _albumService = albumService;
            _userService = userService;
        }

        // GET: api/album
        [HttpGet]
        public ActionResult<IEnumerable<GetAlbumDto>> Get()
        {
            var albums = _albumService.GetAlbums();

            if (albums == null)
            {
                return NotFound();
            }

            var albumsGetDto = new List<GetAlbumDto>();
            albumsGetDto = _mapper.Map<List<GetAlbumDto>>(albums);

            albumsGetDto.ForEach(c => c.Photos = new List<GetPhotoDto>(
               _mapper.Map<IEnumerable<GetPhotoDto>>(_albumService.GetAlbumPhotos(c.Id).ToList())));

            albumsGetDto.ForEach(c => c.OwnerName = _albumService.GetAlbumById(c.Id).User.UserName);

            return Ok(albumsGetDto);
        }
            

        // GET: api/album/{id}
        [HttpGet("{id}")]
        public ActionResult<GetAlbumDto> Get(int id)
        {
            var album = _albumService.GetAlbumById(id);
            if (album == null)
            {
                return NotFound();
            }
            
            var albumGetDto = _mapper.Map<GetAlbumDto>(album);
            albumGetDto.Photos = new List<GetPhotoDto>();
            
            var list = new List<Photo>();

            list.AddRange(_albumService.GetAlbumPhotos(id).ToList());

            var DtoList = _mapper.Map<List<GetPhotoDto>>(list);

            albumGetDto.Photos.AddRange(DtoList);
            albumGetDto.Photos.ForEach(c => c.AlbumName = album.Name);
            albumGetDto.OwnerName = album.User.UserName;

            return Ok(albumGetDto);
        }

        // POST: api/album
        [HttpPost]
        public ActionResult PostAlbum([FromBody] Album album)
        {
            _albumService.AddAlbum(album);
            return Ok();
        }

        // DELETE: api/album/{id}
        [HttpDelete("{id}")]
        public void DeleteAlbum(int id)
        {
            _albumService.DeleteAlbum(id);
        }

        // PUT: api/album
        [HttpPut]
        public ActionResult<GetAlbumDto> PutAlbum([FromBody] Album album)
        {
            _albumService.UpdateAlbum(album);
            return Ok(_mapper.Map<GetAlbumDto>(album));
        }

        // PATCH: api/album/{id}
        [HttpPatch("{id}")]
        public StatusCodeResult PatchAlbum(int id, [FromBody] JsonPatchDocument<Album> patch)
        {
            var album = _albumService.GetAlbumById(id);
            if (album != null)
            {
                patch.ApplyTo(album);
                return Ok();
            }
            return NotFound();
        }
    }
}