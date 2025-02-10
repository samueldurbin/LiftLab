using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getallusers")] // api endpoint which is referenced in the front end
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetUsers(); // calls the get method in the services
            return Ok(users);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.LoginAuthentication(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized(new { Message = "Incorrect username or password, please try again" });
            }

            return Ok(new { Username = user.Username });
        }

        [HttpPost("createuser")]  // api endpoint
        public async Task<IActionResult> CreateNewUser([FromBody] Users request)
        {
            var newUser = await _userService.CreateUser(request); // creates a new post

            if (newUser == null)
            {
                return BadRequest(new { Message = "Experienced an Error creating the new user" });
            }

            return Ok(new { Message = "Your account has been created successfully!", Id = newUser.UserId });
        }

    }
}
