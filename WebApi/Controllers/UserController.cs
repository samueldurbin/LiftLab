using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase   // inherits from the controller base
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]  
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.AuthenticateUser(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            return Ok(new { Username = user.Username });
        }

        [HttpPost("register")]  // api endpoint
        public async Task<IActionResult> Register([FromBody] UserModel request)
        {
            var existingUser = await _userService.GetUsername(request.Username); 

            if (existingUser != null)  // checks to see if the user already exists
            {
                return Conflict(new { Message = "This Username already exists!" });
            }

            var newUser = await _userService.RegisterUser(request); // creates a new user

            if (newUser == null)
            {
                return BadRequest(new { Message = "Experienced an Error creating the user" }); // checks if the new created user will equal null
            }

            return Ok(new { Message = "Your Account has been created successfully!", Username = newUser.Username });
        }
    }

}
