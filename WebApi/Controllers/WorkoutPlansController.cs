using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // WorkoutPlans for the url requests
    public class WorkoutPlansController : ControllerBase 
    {
        private readonly IWorkoutPlansService _workoutPlansService;

        public WorkoutPlansController(IWorkoutPlansService workoutPlansService) // creates instance of the workoutplan service
        {
            _workoutPlansService = workoutPlansService;
        }

        [HttpGet("getallplans")] // request to get all of the workout plans from the database
        public async Task<IActionResult> GetAllPlans()  // basic method to retrieve all the plans
        {
            var plans = await _workoutPlansService.GetPlans();
            return Ok(plans);
        }

        [HttpGet("getworkoutplansbyuser/{userId}")] // this gets the workout plans that are associated with a userid
        public async Task<IActionResult> GetPlansByUserId(int userId)
        {
            var plans = await _workoutPlansService.GetPlansByUser(userId); // function to get plans by userid in services
            return Ok(plans);
        }

        [HttpGet("getplanworkouts/{planId}")] // this gets the associated workout ids within a plan
        public async Task<IActionResult> GetPlanWorkoutsByPlanId(int planId)
        {
            var plans = await _workoutPlansService.GetPlanWorkoutsByPlan(planId); // function to get workoutids plans by userid in services
            return Ok(plans);
        }


        [HttpPost("createplan")] // create plan api endpoint request
        public async Task<IActionResult> CreatePlanWithWorkouts([FromBody] CreateWorkoutPlan request) // creates workout plan from the inserted body
        {
            try
            {
                var newPlan = await _workoutPlansService.CreatePlan(
                    new WorkoutPlans
                    {
                        WorkoutPlanName = request.WorkoutPlanName, // workoutplan name 
                        UserId = request.UserId // in the backend this needs to be manually inserted but in the front end the preferences does this
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
