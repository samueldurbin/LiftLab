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

            return Ok(new { Message = "Your post has been created successfully!", Id = newPost.NutritionPostId }); // success message
        }

        [HttpGet("getallnutritionposts")] // api endpoint which is referenced in the front end
        public async Task<IActionResult> GetAllPosts() // gets all nutritionposts
        {
            var posts = await _nutritionPostService.GetPosts(); // calls method in services
            return Ok(posts);

        }
        
        // Comments for Nutrition Posts -----------------------------------------------------------------------------------------------


        [HttpPost("addcomment")] // endpoint for adding comments
        public async Task<IActionResult> AddComment([FromBody] NutritionPostComments request) // gets data from json body request
        {
            request.Username = "admin"; // hardcoded username for now

            var newComment = await _nutritionPostService.AddComment(request); // calls the add comment method in service
            return Ok(newComment);
        }

        [HttpGet("comments/{nutritionPostId}")] // endpoint for comments and related postid
        public async Task<IActionResult> GetAllComments(int nutritionPostId) // method to get all comments
        {
            var comments = await _nutritionPostService.GetComments(nutritionPostId); // method from services
            return Ok(comments);
        }

        [HttpDelete("deletecomment{id}")] // deletes the comment id and its related data
        public async Task<IActionResult> DeleteComment(int id) // deletes the comment through passed in id
        {
            bool isDeleted = await _nutritionPostService.DeleteComment(id); // method to delete comment in the service class
            if (!isDeleted)
            {
                return BadRequest("Your Comment has not been found, please try again"); // error message
            }

            return Ok("Your comment has been deleted"); // success message
        }

        [HttpPut("updatecomment{id}")] // update comment endpoint, the FitnessPostCommentId will be included into the parameters to be updated
        public async Task<IActionResult> UpdateComment([FromBody] NutritionPostComments updatedComment) // json body of the comment
        {
            var comment = await _nutritionPostService.UpdateComments(updatedComment); // update comment method from services

            if (comment == null) // method to see if the required comment to update exists
            {
                return BadRequest("Comment not found"); // exception message if comment does not exist
            }

            return Ok(comment);
        }

    }
}
