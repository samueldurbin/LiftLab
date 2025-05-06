using Microsoft.AspNetCore.Mvc; // imports ASP.NET Core functionality
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // this sets the base route for the controller's endpoints, removes the word Controller so the endpoint would be CommunityPosts
    public class CommunityPostsController : ControllerBase // base class
    {
        private readonly ICommunityPostService _communityPostService; // dependency injection

        public CommunityPostsController(ICommunityPostService communityPostService) // creates an instance of the IService
        {
            _communityPostService = communityPostService;
        }

        #region CommunityPosts

        [HttpPost("createpost")] // api endpoint to create community post
        public async Task<IActionResult> CreateNewCommunityPost([FromBody] CommunityPost communityPost) // json body in the api request
        {
            var newPost = await _communityPostService.CreatePost(communityPost); // creates a new post

            if (newPost == null) // checks if posts does not exist, error with request
            {
                return BadRequest("Error when creating the new post"); // error exception if post is null (hasnt been correctly retrieved)
            }

            return Ok(new { Message = "Your post has been created successfully!", Id = newPost.CommunityPostId }); // returns a success message alongside what has been added to the db through the apia
        }

        [HttpGet("getallposts")] // api endpoint which is referenced in the front end
        public async Task<IActionResult> GetCommunityPosts()  // method to get all posts for the community view, this is more for testing
        {
            var posts = await _communityPostService.GetPosts(); // calls the get method in the services

            return Ok(posts); // returns a list of posts
        }

        [HttpGet("getpostsbyuser/{userId}")] // this endpoint is for the profile side of the application, where it gets all the posts related to the user
        public async Task<IActionResult> GetPostsByUser(int userId)
        {
            var userPosts = await _communityPostService.GetPostsByUserId(userId);

            return Ok(userPosts); // returns all the users posts
        }

        [HttpDelete("deletepost/{postId}/{userId}")]
        public async Task<IActionResult> DeleteCommunityPost(int postId, int userId) // postid with the userid
        {
            var deletedPost = await _communityPostService.DeletePost(postId, userId); // calls method from service

            if (!deletedPost) // checks for fail
            {
                return BadRequest("Could not delete this post. Please try again");
            }

            return Ok("Post deleted successfully!");
        }

        #endregion

        #region Comments

        [HttpGet("comments/{communityPostId}")] // endpoint for comments and associated postid
        public async Task<IActionResult> GetAllComments(int communityPostId) // method to get all comments
        {
            var comments = await _communityPostService.GetComments(communityPostId); // method from services to get all comments

            return Ok(comments); // returns list of comments
        }

        [HttpPost("addcomment")] // endpoint for http post to add a comment
        public async Task<IActionResult> AddComment([FromBody] AddNewCommentDTO request) // this has changed to a dto post request 
        {
            // this has been changed to a dto in order to reduce the request body as before it had the entire community post object
            var newComment = await _communityPostService.AddComment(new CommunityPostComments // dto into model to match table in database
            {
                CommunityPostId = request.CommunityPostId, // dto to model and calls the service to add it
                Username = request.Username,
                Comment = request.Comment
            });

            return Ok(newComment); // returns http 200 ok and newly created comment
        }

        #endregion

    }

}
