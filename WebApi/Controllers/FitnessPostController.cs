using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApi.Services;
using Shared.Models;

namespace WebApi.Controllers
{
    [ApiController] // apicontroller
    [Route("api/[controller]")] // url for api which uses the controller name as the url
    public class FitnessPostController : ControllerBase // base class
    {
        private readonly IFitnessPostService _fitnessPostService; // dependency injection

        public FitnessPostController(IFitnessPostService fitnessPostService) // creates an instance of the IService
        {
            _fitnessPostService = fitnessPostService;
        }

        [HttpPost("createfitnesspost")]  // api endpoint to create fitness post
        public async Task<IActionResult> CreatePostAsync([FromBody] FitnessPost request) // json body in the api request
        {
            var newPost = await _fitnessPostService.CreatePost(request); // creates a new post

            if (newPost == null) // checks if posts does not exist, error with request
            {
                return BadRequest("Error when creating new post" );  // error exception if post is null (hasnt been correctly retrieved)
            }

            return Ok(new { Message = "Your post has been created successfully!", Id = newPost.FitnessPostId }); // returns a success message alongside what has been added to the db through the apia
        }

        [HttpGet("getallfitnessposts")] // api endpoint which is referenced in the front end
        public async Task<IActionResult> GetPosts() // method to get all fitnessposts for the community view
        {
            var posts = await _fitnessPostService.GetPosts(); // calls the get method in the services
            return Ok(posts);

        }

        // -----------------------------Comments for Fitness Post Section ---------------------------------------------------------------------

        [HttpPost("addcomment")] // endpoint for adding comments
        public async Task<IActionResult> AddComment([FromBody] FitnessPostComments request) // gets data from json body request
        {
            request.Username = "admin"; // hardcoded username for now

            var newComment = await _fitnessPostService.AddComment(request); // calls the add comment method in service
            return Ok(newComment);
        }

        [HttpDelete("deletecomment{id}")] // deletes the comment id and its related data
        public async Task<IActionResult> DeleteComment(int id) // deletes the comment through passed in id
        {
            bool isDeleted = await _fitnessPostService.DeleteComment(id); // method to delete comment in the service class
            if (!isDeleted)
            {
                return BadRequest("Your Comment has not been found, please try again"); // error message
            }

            return Ok("Your comment has been deleted"); // success message
        }

        [HttpPut("updatecomment{id}")] // update comment endpoint, the FitnessPostCommentId will be included into the parameters to be updated
        public async Task<IActionResult> UpdateComment([FromBody] FitnessPostComments updatedComment) // json body of the comment
        {
            var comment = await _fitnessPostService.UpdateComments(updatedComment); // update comment method from services

            if (comment == null) // method to see if the required comment to update exists
            {
                return BadRequest("Comment not found"); // exception message if comment does not exist
            }

            return Ok(comment);
        }
    }
}
