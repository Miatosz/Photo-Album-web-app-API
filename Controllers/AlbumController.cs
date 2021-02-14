using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImageAlbumAPI.Dtos.GetDtos;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ImageAlbumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IAlbumService _albumService;   

        public AlbumController(IAlbumService albumService, IMapper mapper, UserManager<User> userManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _albumService = albumService;
        }

        // GET: api/album
        [HttpGet]
        public ActionResult<IEnumerable<GetAlbumDto>> Get()
        {
            var albums = _albumService.GetAlbums();

            var albumsGetDto = new List<GetAlbumDto>();
            albumsGetDto = _mapper.Map<List<GetAlbumDto>>(albums);

            albumsGetDto.ForEach(c => c.Photos = new List<GetPhotoDto>(
                _mapper.Map<IEnumerable<GetPhotoDto>>(_albumService.GetAlbumPhotos(c.Id).ToList())));

            albumsGetDto.ForEach(c => c.OwnerName = _albumService.GetAlbumById(c.Id).User.UserName);

            return Ok(albumsGetDto);
        }
            

        // GET: api/album/{id}
        [HttpGet("{id}")]
        public ActionResult<GetAlbumDto> GetById(int id)
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
        [Authorize]
        public ActionResult<Album> PostAlbum([FromBody] Album album)
        {
            album.User = GetCurrentLoggedUser().Result;
            _albumService.AddAlbum(album);
            return Ok(album);
        }

        // DELETE: api/album/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public void DeleteAlbum(int id)
        {
            var album = _albumService.GetAlbumById(id);
            var user = GetCurrentLoggedUser().Result;

            if (user.Albums.Contains(album))
            {
                _albumService.DeleteAlbum(id);
            }            
        }

        // PUT: api/album
        [HttpPut]
        public ActionResult<GetAlbumDto> PutAlbum([FromBody] Album album)
        {
            var user = GetCurrentLoggedUser().Result;

            if (user.Albums.FirstOrDefault(c => c.Id == album.Id) != null)
            {
                _albumService.UpdateAlbum(album);
            }            
            return Ok(_mapper.Map<GetAlbumDto>(album));
        }

        // PATCH: api/album/{id}
        [HttpPatch("{id}")]
        public StatusCodeResult PatchAlbum(int id, [FromBody] JsonPatchDocument<Album> patch)
        {
            var album = _albumService.GetAlbumById(id);
            var user = GetCurrentLoggedUser().Result;

            if (user.Albums.Contains(album))
            {
                if (album != null)
                {
                    patch.ApplyTo(album);
                    return Ok();
                }
                return NotFound();
            }            
            return BadRequest();
        }

        private async Task<User> GetCurrentLoggedUser()
        {   
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            user.Albums = _albumService.Albums.Where(c => c.User.Id == user.Id).ToList();

            return user;
        }
    }
}