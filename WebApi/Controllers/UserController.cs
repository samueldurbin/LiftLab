using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserModel request)
        {
            var user = await _userService.ValidateUser(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            return Ok(new { Username = user.Username });
        }
    }

}
