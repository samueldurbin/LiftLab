using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApi.Services;
using WebApi.Models;
using Shared.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class FitnessPostController : ControllerBase
    {
        private readonly IFitnessPostService _fitnessPostService;

        public FitnessPostController(IFitnessPostService fitnessPostService)
        {
            _fitnessPostService = fitnessPostService;
        }

        [HttpPost("createpost")]  // api endpoint
        public async Task<IActionResult> CreatePostAsync([FromBody] FitnessPost request)
        {
            var newPost = await _fitnessPostService.CreatePost(request); // creates a new post

            if (newPost == null)
            {
                return BadRequest(new { Message = "Experienced an Error creating the post" }); 
            }

            return Ok(new { Message = "Your post has been created successfully!", Id = newPost.Id });
        }
    }
}
