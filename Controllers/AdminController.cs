using System.Linq;
using System.Threading.Tasks;
using ImageAlbumAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageAlbumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles="Admin")]
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
        public ActionResult<User> GetUserById(string id)
        {
            var user = _userManager.Users.FirstOrDefault(c => c.Id == id);
            if (user == null)
                return NotFound();

            return Ok(_userManager.Users.FirstOrDefault(c => c.Id == id));
        } 
    }
}
