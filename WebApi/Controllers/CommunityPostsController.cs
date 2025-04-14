using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController] // apicontroller
    [Route("api/[controller]")] // url for api which uses the controller name as the url
    public class CommunityPostsController : ControllerBase // base class
    {
        private readonly ICommunityPostService _communityPostService; // dependency injection

        public CommunityPostsController(ICommunityPostService communityPostService) // creates an instance of the IService
        {
            _communityPostService = communityPostService;
        }

        [HttpPost("createcommunitypost")] // api endpoint to create fitness post
        public async Task<IActionResult> CreatePostAsync([FromBody] CommunityPost request) // json body in the api request
        {
            var newPost = await _communityPostService.CreatePost(request); // creates a new post

            if (newPost == null) // checks if posts does not exist, error with request
            {
                return BadRequest("Error when creating new post"); // error exception if post is null (hasnt been correctly retrieved)
            }

            return Ok(new { Message = "Your post has been created successfully!", Id = newPost.CommunityPostId }); // returns a success message alongside what has been added to the db through the apia
        }

        [HttpGet("getallcommunityposts")] // api endpoint which is referenced in the front end
        public async Task<IActionResult> GetPosts()  // method to get all fitnessposts for the community view
        {
            var posts = await _communityPostService.GetPosts(); // calls the get method in the services
            return Ok(posts);
        }

        [HttpGet("comments/{communityPostId}")] // endpoint for comments and related postid
        public async Task<IActionResult> GetAllComments(int communityPostId) // method to get all comments
        {
            var comments = await _communityPostService.GetComments(communityPostId); // method from services
            return Ok(comments);
        }

        [HttpPost("addcomment")] // endpoint for http post to add a comment
        public async Task<IActionResult> AddComment([FromBody] AddNewCommentDTO request) // this has changed to a dto post request 
        {
            // this has been changed to a dto in order to reduce the request body as before it had the entire fitnesspost object
            var newComment = await _communityPostService.AddComment(new CommunityPostComments // dto into model to match table in database
            {
                CommunityPostId = request.CommunityPostId, // dto to model
                Username = request.Username,
                Comment = request.Comment
            });

            return Ok(newComment); // returns http 200 ok and newly created comment
        }

        [HttpDelete("deletecomment{id}")] // deletes the comment id and its related data
        public async Task<IActionResult> DeleteComment(int id) // deletes the comment through passed in id
        {
            bool isDeleted = await _communityPostService.DeleteComment(id); // method to delete comment in the service class
            if (!isDeleted)
            {
                return BadRequest("Your Comment has not been found, please try again"); // error message
            }

            return Ok("Your comment has been deleted"); // success message
        }
         
        [HttpPut("updatecomment{id}")] // update comment endpoint, the FitnessPostCommentId will be included into the parameters to be updated
        public async Task<IActionResult> UpdateComment([FromBody] CommunityPostComments updatedComment)  // json body of the comment
        {
            var comment = await _communityPostService.UpdateComments(updatedComment); // update comment method from services

            if (comment == null) // method to see if the required comment to update exists
            {
                return BadRequest("Comment not found"); // exception message if comment does not exist
            }

            return Ok(comment);
        }

        [HttpPost("like/{postId}/{userId}")] // this endpoint uses the id of the post and the userid who liked it
        public async Task<IActionResult> LikePost(int postId, int userId) 
        {
            var result = await _communityPostService.LikePost(postId, userId); 

            if (!result) // this wouldve checked if the post was liked twice
            {
                return BadRequest(""); // future development needs to prevent a badrequest from happening, as its not very fluent to recieve errors when double liking a post
            }

            return Ok(result); // http 200 ok
        }

        [HttpGet("getpostsbyuser/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(int userId)
        {
            var userPosts = await _communityPostService.GetPostsByUserId(userId);
            return Ok(userPosts);
        }
    }

}
