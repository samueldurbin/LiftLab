using Microsoft.AspNetCore.Mvc;
using Shared.Models;

using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController] // asp.net core
    [Route("api/[controller]")]  // endpoints, in this case api/Pals
    public class PalsController : ControllerBase
    {
        private readonly IPalsService _palsService; // variable storing the interface

        public PalsController(IPalsService palsService) // dependcy injection, assigns service
        {
            _palsService = palsService; // controller can use service
        }

        [HttpGet("{userId}")] // api endpoint which includes what userid is inserted
        public async Task<IActionResult> GetPalsCountForUser (int userId) // accessbile for api, returns a response ok. Userid in endpoint comes from Userid in route
        {
            int usersPals = await _palsService.GetMyPals(userId); // calls get pals method to get the count of pals from the database
            // userpals stores the result of how many users there are
            // no check to see if a user has no pals as they are not a requirement
            return Ok(usersPals); // returns a 200 response with the count of followers to show postman success. HTTP response
        }

        [HttpPost("AddPals")] // endpoint for the api, post request for creating
        public async Task<IActionResult> AddPals([FromBody] Pals pals) // request body for the api request, in this case the ids
        {
            var newPal = await _palsService.AddPals(pals); // calls service method to add a new pal

            if (newPal == null)
            {
                return BadRequest( "Experienced an Error when adding pal");  // to prevent an empty pal from being added
            }

            return Ok("Created Successfully"); // response message to show its success
           
        }

   
    }
}
