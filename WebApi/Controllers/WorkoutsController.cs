using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Services;
using WebApi.Models;
using Shared.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutsController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpGet("getallworkouts")] 
        public async Task<IActionResult> GetAllWorkouts()
        {
            var workouts = await _workoutService.GetWorkouts();
            return Ok(workouts);

        }

    }
}
