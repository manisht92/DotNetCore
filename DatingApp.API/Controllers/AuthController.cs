using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegister userForRegister)
        {
            var user = userForRegister.Username.ToLower();
            if (await _repo.UserExist(user))
                ModelState.AddModelError("Username","Username already exist");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToCreate = new User
            {
                Username = userForRegister.Username.ToLower()
            };

            var createUser = await _repo.Register(userToCreate, userForRegister.Password);
            return StatusCode(201);
        }
    }
}