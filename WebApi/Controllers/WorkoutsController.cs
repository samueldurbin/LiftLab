using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

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
