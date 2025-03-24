using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Services;
using Shared.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // workouts as a url
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutsController(IWorkoutService workoutService) // creates an instance of workouts service
        {
            _workoutService = workoutService;
        }

        [HttpGet("getallworkouts")]  // gets all workouts
        public async Task<IActionResult> GetAllWorkouts() 
        {
            var workouts = await _workoutService.GetWorkouts(); // simple method to retrieve all workouts
            return Ok(workouts);

        }

    }
}
