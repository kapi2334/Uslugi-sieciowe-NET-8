using BlogCMS.Interfaces;
using BlogCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController: ControllerBase
    {
        private readonly IRepository<LoginModel> _loginRepository;

        public RegisterController(IRepository<LoginModel> loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(int id)
        {
            // Tutaj musiałbyś dodać logikę pobierania po ID, np:
            var user = await _loginRepository.GetByIdAsync(id);
            if(user == null) return NotFound();
            return Ok(user);


        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            loginModel.Role = "Admin";
            var newLoginModelId = await _loginRepository.AddAsync(loginModel);
            return CreatedAtAction(nameof(GetUser), new { Id = newLoginModelId }, loginModel);
        }
    }
}
