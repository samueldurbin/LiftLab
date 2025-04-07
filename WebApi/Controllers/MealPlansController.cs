using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // WorkoutPlans for the url requests
    public class MealPlansController : Controller
    {
        private readonly IMealPlansService _mealPlansService;

        public MealPlansController(IMealPlansService mealPlansService)
        {
            _mealPlansService = mealPlansService;
        }

        [HttpGet("getallmealplans")]
        public async Task<IActionResult> GetAllMealPlans()
        {
            var plans = await _mealPlansService.GetAllMealPlans();
            return Ok(plans);
        }

        [HttpGet("getmealplansbyuser/{userId}")]
        public async Task<IActionResult> GetMealPlansByUser(int userId)
        {
            var plans = await _mealPlansService.GetMealPlansByUser(userId);
            return Ok(plans);
        }

        [HttpGet("getmealsbyplan/{planId}")]
        public async Task<IActionResult> GetMealsByPlanId(int planId)
        {
            var meals = await _mealPlansService.GetMealsByPlanId(planId);
            return Ok(meals);
        }

        [HttpPost("createmealplan")]
        public async Task<IActionResult> CreateMealPlan([FromBody] CreateMealPlanDTO request)
        {
            try
            {
                var newPlan = await _mealPlansService.CreateMealPlan( // create a new meal plan and save the meals
                    new MealPlan
                    {
                        MealPlanName = request.MealPlanName,
                        UserId = request.UserId
                    },
                    request.Meals
                );

                return Ok(new { Message = "Meal Plan has been created!", PlanId = newPlan.MealPlanId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
