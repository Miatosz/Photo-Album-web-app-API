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
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IUserRepo _repo;

        public UserController(IUserRepo repo, AppDbContext ctx, IMapper mapper)
        {
            _mapper = mapper;
            _context = ctx;
            _repo = repo;
        }

        // GET: /api/user
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            var users = _context.Users;

            if (users == null)
            {
                return NotFound();
            }
            var usersGetDto = new List<GetUserDto>();
            usersGetDto = _mapper.Map<List<GetUserDto>>(users);


            usersGetDto.ForEach(c => c.Albums = new List<GetAlbumDto>(
               _mapper.Map<IEnumerable<GetAlbumDto>>(_context.Albums
                                                            .Include(d => d.User)
                                                            .Where(d => d.UserId == c.Id)
                                                            .ToList())
                ));

            usersGetDto.ForEach(c => c.Albums
                                        .ForEach(d => d.OwnerName = c.UserName));
            
            usersGetDto.ForEach(c => c.Albums
                                        .ForEach(d => d.Photos = new List<GetPhotoDto>(
                                            _mapper.Map<IEnumerable<GetPhotoDto>>(_context.Photos
                                                                                    .Include(d => d.Album)
                                                                                    .Where(d => d.AlbumId == c.Id)
                                                                                    .ToList())
                                        )));
            return Ok(usersGetDto);
        }

        // GET: /api/user/{id}
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = _repo.Users.FirstOrDefault(c => c.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var userGetDto = _mapper.Map<GetUserDto>(user);

            userGetDto.Albums = new List<GetAlbumDto>();

            var list = new List<Album>();

            list.AddRange(_context.Albums
                                .Include(c => c.User)
                                .Where(c => c.UserId == user.Id)
                                .ToList());

            var DtoList = _mapper.Map<List<GetAlbumDto>>(list);

            userGetDto.Albums.AddRange(DtoList);
            userGetDto.Albums.ForEach(c => c.OwnerName = user.UserName);
            userGetDto.Albums.ForEach(c => c.Photos = new List<GetPhotoDto>(
                                            _mapper.Map<IEnumerable<GetPhotoDto>>(_context.Photos
                                                                                    .Include(d => d.Album)
                                                                                    .Where(d => d.AlbumId == c.Id)
                                                                                    .ToList())
                                        ));

            return Ok(userGetDto);
        }  

        // POST: /api/user
        public User PostUser([FromBody] User user)
        {
            _repo.AddUser(user);
            return user;
        }

        // PUT: /api/user
        [HttpPut]
        public User PutUser([FromBody] User user)
        {
            _repo.UpdateUser(user);
            return user;
        }


        // PATCH: /api/user/{id}
        [HttpPatch("{id}")]
        public StatusCodeResult UserPatch(int id, [FromBody] JsonPatchDocument<User> patch)
        {
            User user = _repo.Users.FirstOrDefault(c => c.Id == id);
            if (user != null)
            {
                patch.ApplyTo(user);
                return Ok();
            }
            return NotFound();
        }

        // DELETE: /api/user/{id}
        [HttpDelete("{id}")]
        public void DeleteUser(int id)
        {
            _repo.DeleteUser(id);
        }
    }
}