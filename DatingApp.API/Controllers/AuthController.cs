using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
            if (string.IsNullOrEmpty(userForRegister.Username))
                userForRegister.Username = userForRegister.Username.ToLower();

            if (await _repo.UserExist(userForRegister.Username))
                ModelState.AddModelError("Username", "Username already exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToCreate = new User
            {
                Username = userForRegister.Username.ToLower()
            };

            var createUser = await _repo.Register(userToCreate, userForRegister.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserForLogin userForLogin)
        {
            // try
            // {
                //throw new Exception("Computer says no!");

                var userRepo = await _repo.Login(userForLogin.Username.ToLower(), userForLogin.Password);
                if (userRepo == null)
                    return Unauthorized();

                // generate Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("super secret key");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, userRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name,userRepo.Username)
                }),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { tokenString });
            // }
            // catch
            // {
            //     return StatusCode(500, "Computer really says no!");
            // }
        }
    }
}