using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController] // apicontroller
    [Route("api/[controller]")] // url for api which uses the controller name as the url
    public class NutritionPostsController : ControllerBase
    {
        private readonly INutritionPostService _nutritionPostService; // dependency injection

        public NutritionPostsController(INutritionPostService nutritionPostService) // creates an instance of the IService
        {
            _nutritionPostService = nutritionPostService;
        }

        [HttpPost("createnutritionpost")]  // api endpoint to create fitness post
        public async Task<IActionResult> CreatePost([FromBody] NutritionPost request) // json body in the api request
        {
            var newPost = await _nutritionPostService.CreatePosts(request); // creates a new post

            if (newPost == null) // checks if posts does not exist, error with request
            {
                return BadRequest("Error when creating new post");  // error exception if post is null (hasnt been correctly retrieved)
            }

            return Ok("Your post has been created successfully!"); // returns a success message alongside what has been added to the db through the apia
        }

        [HttpGet("getallnutritionposts")] // api endpoint which is referenced in the front end
        public async Task<IActionResult> GetAllPosts() // method to get all fitnessposts for the community view
        {
            var posts = await _nutritionPostService.GetPosts(); // calls the get method in the services
            return Ok(posts);

        }

    }
}
