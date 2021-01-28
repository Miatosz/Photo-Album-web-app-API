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
    public class AlbumController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IAlbumRepo _repo;

        public AlbumController(IAlbumRepo repo, AppDbContext ctx, IMapper mapper)
        {
            _mapper = mapper;
            _context = ctx;
            _repo = repo;
        }

        // GET: api/album
        [HttpGet]
        public ActionResult<IEnumerable<GetAlbumDto>> Get()
        {
            var albums = _context.Albums.Include(c => c.User);

            if (albums == null)
            {
                return NotFound();
            }

            var albumsGetDto = new List<GetAlbumDto>();
            albumsGetDto = _mapper.Map<List<GetAlbumDto>>(albums);

            albumsGetDto.ForEach(c => c.Photos = new List<GetPhotoDto>(
               _mapper.Map<IEnumerable<GetPhotoDto>>(_context.Photos
                                                            .Include(d => d.Album)
                                                            .Where(d => d.AlbumId == c.Id)
                                                            .ToList())
            ));

            albumsGetDto.ForEach(c => c.OwnerName = albums.First(d => d.Id == c.Id).User.UserName);

           

            return Ok(albumsGetDto);
        }
            

        // GET: api/album/{id}
        [HttpGet("{id}")]
        public ActionResult<GetAlbumDto> Get(int id)
        {
            var album = _context.Albums
                                .Include(c => c.User)
                                .First(c => c.Id == id);
            if (album == null)
            {
                return NotFound();
            }
            
            var albumGetDto = _mapper.Map<GetAlbumDto>(album);
            albumGetDto.Photos = new List<GetPhotoDto>();
            
            var list = new List<Photo>();

            list.AddRange(_context.Photos
                                .Include(c => c.Album)
                                .Where(c => c.AlbumId == album.Id)
                                .ToList());

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
            _repo.AddAlbum(album);
            return Ok();
        }

        // DELETE: api/album/{id}
        [HttpDelete("{id}")]
        public void DeleteAlbum(int id)
        {
            _repo.DeleteAlbum(id);
        }

        // PUT: api/album
        [HttpPut]
        public ActionResult<GetAlbumDto> PutAlbum([FromBody] Album album)
        {
            _repo.UpdateAlbum(album);
            return Ok(_mapper.Map<GetAlbumDto>(album));
        }

        // PATCH: api/album/{id}
        [HttpPatch("{id}")]
        public StatusCodeResult PatchAlbum(int id, [FromBody] JsonPatchDocument<Album> patch)
        {
            var album = _repo.Albums.First(c => c.Id == id);
            if (album != null)
            {
                patch.ApplyTo(album);
                return Ok();
            }
            return NotFound();
        }
    }
}