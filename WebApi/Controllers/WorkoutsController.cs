using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutsService _workoutsService;

        public WorkoutsController(IWorkoutsService workoutsService)
        {
            _workoutsService = workoutsService;
        }

        [HttpGet("workouts")]
        public async Task<IActionResult> GetWorkouts()
        {
            var workouts = await _workoutsService.GetWorkouts();
            return Ok(workouts);

        }
    }
}
