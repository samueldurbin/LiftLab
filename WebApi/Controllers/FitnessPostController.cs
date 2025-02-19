using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApi.Services;
using Shared.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FitnessPostController : ControllerBase
    {
        private readonly IFitnessPostService _fitnessPostService;

        public FitnessPostController(IFitnessPostService fitnessPostService)
        {
            _fitnessPostService = fitnessPostService;
        }

        [HttpPost("createpost")]  // api endpoint
        public async Task<IActionResult> CreatePostAsync([FromBody] FitnessPost request) // json body in request
        {
            var newPost = await _fitnessPostService.CreatePost(request); // creates a new post

            if (newPost == null) // checks if posts does not exist, error with request
            {
                return BadRequest(new { ResponseMessage = "Experienced an Error creating the new post" }); 
            }

            return Ok(new { Message = "Your post has been created successfully!", Id = newPost.Id });
        }

        [HttpGet("fitnessposts")] // api endpoint which is referenced in the front end
        public async Task<IActionResult> GetPosts() 
        {
            var posts = await _fitnessPostService.GetPosts(); // calls the get method in the services
            return Ok(posts);

        }
    }
}
