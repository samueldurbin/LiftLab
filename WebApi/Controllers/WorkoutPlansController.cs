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

        [HttpPost("adduserworkoutplan/{planId}/{userId}")] // api endpoints for a http post request, workoutoutplanid to be added by userId
        public async Task<IActionResult> AddExternalWorkoutPlan(int planId, int userId)
        {
            try
            {
                var addedUserPlan = await _workoutPlansService.AddExternalUserWorkoutPlan(planId, userId); // calls function in the service

                return Ok(new { Message = "Workout Plan successfully added to your account!", PlanId = addedUserPlan.WorkoutPlanId }); // return success 200 message with the workoutplanid if success
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // if theres an error return badrequest
            }
        }

        [HttpPost("addworkouttoplan/{planId}")]
        public async Task<IActionResult> AddWorkoutToPlan(int planId, [FromBody] AddWorkoutToPlanDTO dto)  // api endpoint for adding workoutid input into workoutplan
        {
            var addedWorkout = await _workoutPlansService.AddWorkoutToPlan(planId, dto.WorkoutId, dto.Reps, dto.Sets); // calls method from service
            if (!addedWorkout)
            {
                return BadRequest("Workout already added or not found."); // if duplicated or does not ecist through error but this is mainly for testing
            }

            return Ok("Workout added with reps and sets.");
        }

        [HttpDelete("removeworkoutfromplan/{workoutPlanId}/{workoutId}")] // api endpoint for removing workoutid input from workoutplan
        public async Task<IActionResult> RemoveWorkoutFromPlan(int workoutPlanId, int workoutId)
        {
            var deletedWorkout = await _workoutPlansService.DeleteWorkoutFromPlan(workoutPlanId, workoutId); // calls method from service
            if (!deletedWorkout)
            {
                return NotFound("Workout has not found in the workout plan."); // if workout does not ecist through error
            }

            return Ok("Workout has been removed from the workout plan.");
        }


    }
}
