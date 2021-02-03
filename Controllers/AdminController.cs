using System.Linq;
using System.Threading.Tasks;
using ImageAlbumAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageAlbumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private UserManager<User> _userManager;

        public AdminController(UserManager<User> userMgr)
        {
            _userManager = userMgr;
        }

        // GET: /api
        [HttpGet]
        public IQueryable<User> GetUsers() => _userManager.Users;

        // GET: /api/{id}
        [HttpGet("{id}")]
        public User GetUserById(int id) => _userManager.Users.FirstOrDefault(c => c.UserId == id);

        [HttpPost]
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
            return Ok(model);
        }
    }
}