using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController] // apicontroller
    [Route("api/[controller]")] // endpoints for api
    public class NutritionPostsController : ControllerBase
    {
        private readonly INutritionPostService _nutritionPostService; // dependency injection

        public NutritionPostsController(INutritionPostService nutritionPostService) // creates an instance of the IService
        {
            _nutritionPostService = nutritionPostService;
        }

        [HttpPost("createnutritionpost")]  // api endpoint to create nutrition post
        public async Task<IActionResult> CreatePost([FromBody] NutritionPost request) // json body in the api request
        {
            var newPost = await _nutritionPostService.CreatePosts(request); // creates a new post

            if (newPost == null) // checks if posts does not exist, error with request
            {
                return BadRequest("Error when creating new post");  // error exception message
            }

            return Ok("Your post has been created successfully!"); // success message
        }

        [HttpGet("getallnutritionposts")] // api endpoint which is referenced in the front end
        public async Task<IActionResult> GetAllPosts() // gets all nutritionposts
        {
            var posts = await _nutritionPostService.GetPosts(); // calls method in services
            return Ok(posts);

        }

    }
}
