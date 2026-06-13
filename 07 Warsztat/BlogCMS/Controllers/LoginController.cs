using BlogCMS.Dev;
using BlogCMS.Interfaces;
using BlogCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRepository<LoginModel> _loginRepository;
        public LoginController(IConfiguration config, IRepository<LoginModel> loginRepository)
        {
            _loginRepository = loginRepository;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginModel userLogin)
        {
            var user = await AuthenticateAsync(userLogin);

            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }

            return NotFound("user not found");
        }

        // To generate token
        private string GenerateToken(LoginModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //To authenticate user
        private async Task<LoginModel?> AuthenticateAsync(LoginModel userLogin)
        {

            var currentUser = UserConstants.Users.FirstOrDefault(x =>
                x.Username.ToLower() == userLogin.Username.ToLower() &&
                x.Password == userLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }

            var allUsersFromDb = await _loginRepository.GetAllAsync();

            var dbUser = allUsersFromDb.FirstOrDefault(x =>
                x.Username.ToLower() == userLogin.Username.ToLower() &&
                x.Password == userLogin.Password);

            return dbUser; 
        }
    }
}
