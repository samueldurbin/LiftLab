using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController] // apicontroller
    [Route("api/[controller]")] // url for api which uses the controller name as the url
    public class FriendsController : Controller
    {
        private readonly IFriendService _friendService; // dependency injection

        public FriendsController(IFriendService friendService) // creates an instance of the IService
        {
            _friendService = friendService;
        }

        [HttpPost("addfriend")] // api endpoint
        public async Task<IActionResult> AddFriend([FromBody] FriendRequest request) // adds friends through json request body
        {
            bool success = await _friendService.AddFriend(request.UserId, request.FriendUserId);

            if (!success) // tests to see if the friendship already exists between the two users or that the request failed
            {
                return BadRequest("The Friend request failed or it already exists.."); // error message

            }

            return Ok("Friend added successfully!"); // success message
        }

    }
}
