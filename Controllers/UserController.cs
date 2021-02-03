using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImageAlbumAPI.Dtos.GetDtos;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Services;
using Microsoft.AspNetCore.Identity;
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
        private readonly IAlbumService _albumService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private IUserValidator<User> _userValidator;

        public UserController(IUserService userService, IAlbumService albumService, 
                                IMapper mapper, UserManager<User> userMgr, 
                                IPasswordHasher<User> passwordHasher,
                                IUserValidator<User> userValidator)
        {
            _mapper = mapper;
            _albumService = albumService;
            _userService = userService;
            _userManager = userMgr;
            _passwordHasher = passwordHasher;
            _userValidator = userValidator;
        }

        // GET: /api/user
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            var users = _userService.GetUsers();

            if (users == null)
            {
                return NotFound();
            }
            var usersGetDto = new List<GetUserDto>();
            usersGetDto = _mapper.Map<List<GetUserDto>>(users);


            usersGetDto.ForEach(c => c.Albums = new List<GetAlbumDto>(
               _mapper.Map<IEnumerable<GetAlbumDto>>(_userService.GetUserAlbums(c.UserId).ToList())));

            usersGetDto.ForEach(c => c.Albums
                                        .ForEach(d => d.OwnerName = c.UserName));
            
            usersGetDto.ForEach(c => c.Albums
                                        .ForEach(d => d.Photos = new List<GetPhotoDto>(
                                            _mapper.Map<IEnumerable<GetPhotoDto>>(_albumService.GetAlbumPhotos(d.Id).ToList()))));
            return Ok(usersGetDto);
        }

        // GET: /api/user/{id}
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            var userGetDto = _mapper.Map<GetUserDto>(user);

            userGetDto.Albums = new List<GetAlbumDto>();

            var list = new List<Album>();

            list.AddRange(_userService.GetUserAlbums(id).ToList());

            var DtoList = _mapper.Map<List<GetAlbumDto>>(list);

            userGetDto.Albums.AddRange(DtoList);
            userGetDto.Albums.ForEach(c => c.OwnerName = user.UserName);
            userGetDto.Albums.ForEach(c => c.Photos = new List<GetPhotoDto>(
                                            _mapper.Map<IEnumerable<GetPhotoDto>>(_albumService.GetAlbumPhotos(c.Id).ToList())
                                        ));
            return Ok(userGetDto);
        }  

        // POST: /api/user
        // public User PostUser([FromBody] User user)
        // {
        //     _userService.AddUser(user);
        //     return user;
        // }
        
        // POST: /api/user
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else 
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return BadRequest(model);
        }



        // PATCH: /api/user/{id}
        [HttpPatch("{id}")]
        public async Task<StatusCodeResult> UserPatch(int id, [FromBody] JsonPatchDocument<User> patch)
        {
            User user = _userManager.Users.FirstOrDefault(c => c.UserId == id);
            if (user != null)
            {
                patch.ApplyTo(user);
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "user not found");
            }
            return NotFound();
        }

        // POST /api/user/{id}/changePassword
        [HttpPost("{id}/changePassword")]
        [Route("{id}/changePassword")]
        public async Task<ActionResult> ChangeUserPassword([FromBody] CreateUserModel model, int id)
        {
            User user = _userManager.Users.FirstOrDefault(c => c.UserId == id);
            //if (user.Email == email)
            //{
                user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                IdentityResult validPass = await _userValidator.ValidateAsync(_userManager, user);
                if (validPass.Succeeded)
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
                else
                {
                    return BadRequest(validPass.Errors);
                }
                return BadRequest(validPass.Errors);
        }

        // DELETE: /api/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            //_userService.DeleteUser(id);
            User user = _userManager.Users.FirstOrDefault(c => c.UserId == id);
            if (user != null)
            {
                IdentityResult result  = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "user not found");
            }
            return Ok(_userManager.Users);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}