using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly IWorkoutPlansService _workoutPlansService;

        public WorkoutPlansController(IWorkoutPlansService workoutPlansService)
        {
            _workoutPlansService = workoutPlansService;
        }

        [HttpGet("getallplans")]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _workoutPlansService.GetPlans();
            return Ok(plans);
        }

        [HttpPost("createplan")]
        public async Task<IActionResult> CreatePlanWithWorkouts([FromBody] CreateWorkoutPlan request)
        {
            try
            {
                var newPlan = await _workoutPlansService.CreatePlan(
                    new WorkoutPlans
                    {
                        WorkoutPlanName = request.WorkoutPlanName,
                        UserId = request.UserId
                    },
                    request.WorkoutIds
                );

                return Ok(new { Message = "Your Workout Plan has been created!", PlanId = newPlan.WorkoutPlanId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
