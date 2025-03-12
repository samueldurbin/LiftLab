using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // created for user in url
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) // creates an instance of users service for the login
        {
            _userService = userService;
        }

        [HttpGet("getallusers")] // api endpoint which is referenced in the front end
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetUsers(); // calls the get method in the services
            return Ok(users);

        }

        [HttpPost("login")] // url for login
        public async Task<IActionResult> Login([FromBody] LoginRequest request) // uses the json request body and compares it with the input within the login authentication request
        {
            var user = await _userService.LoginAuthentication(request.Username, request.Password);
            if (user == null)
            {
                return BadRequest("Incorrect username or password, please try again"); // error message
            }

            return Ok();
        }

        [HttpPost("createuser")]  // api endpoint for creating user
        public async Task<IActionResult> CreateNewUser([FromBody] Users request)
        {
            var newUser = await _userService.CreateUser(request); // creates a new post

            if (newUser == null)
            {
                return BadRequest("Experienced an Error creating the new user");
            }

            return Ok(new { Message = "Your account has been created successfully!", Id = newUser.UserId });
        }

        [HttpDelete("{id}")] // deletes the user that is passed into the id field
        public async Task<IActionResult> DeleteUser(int id) // deletes the user through id
        {
            bool isDeleted = await _userService.DeleteUser(id); // method to delete user in the service class
            if (!isDeleted)
            {
                return BadRequest( "User has not been found" ); // error message
            }

            return Ok(new { Message = "Your account has been deleted"});
        }

        [HttpPut("{id}")] // update user function for the settings page
        public async Task<IActionResult> UpdateUser([FromBody] Users updatedUser) // json body of the user
        {
            var user = await _userService.UpdateUser(updatedUser); // update user method

            if (user == null) // method to see if the required user to update exists
            {
                return BadRequest("User not found" );
            }

            return Ok(user);
        }

    }
}
