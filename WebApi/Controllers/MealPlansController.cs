using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // WorkoutPlans for the url requests
    public class MealPlansController : Controller
    {
        private readonly IMealPlansService _mealPlansService;

        public MealPlansController(IMealPlansService mealPlansService) // dependency injection
        {
            _mealPlansService = mealPlansService;
        }

        [HttpGet("getallmealplans")] // api endpoint foe getting all meal plans created by all users. Mainly for testing
        public async Task<IActionResult> GetAllMealPlans()
        {
            var plans = await _mealPlansService.GetAllMealPlans(); // Ggets all meal plans from the service method
            return Ok(plans); // returns 200 message if success
        }

        [HttpGet("getmealplansbyuser/{userId}")] // api endpoint for geting meal plans created by the input userId
        public async Task<IActionResult> GetMealPlansByUser(int userId)
        {
            var plans = await _mealPlansService.GetMealPlansByUser(userId); // this gets meal plans by input userid
            return Ok(plans);
        }

        [HttpGet("getmealsbyplan/{planId}")] // uses planid input
        public async Task<IActionResult> GetMealsByPlanId(int planId)
        {
            var meals = await _mealPlansService.GetMealsByPlanId(planId); // this method in the service gets all the meals that
            return Ok(meals);
        }

        [HttpPost("createmealplan")]
        public async Task<IActionResult> CreateMealPlan([FromBody] CreateMealPlanDTO request) // creates meal plan with the DTO
        {
            try
            {
                var newPlan = await _mealPlansService.CreateMealPlan(
                    new MealPlans
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

        [HttpPost("createmeal")]
        public async Task<IActionResult> CreateMeals([FromBody] Meals meal) // this creates a single meal, outside of a plan and can be included into plan later
        {
            try
            {
                var createdMeal = await _mealPlansService.CreateMeal(meal);
                return Ok(new { Message = "Meal has been created successfully", MealId = createdMeal.MealId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to create meal", Error = ex.Message });
            }
        }

        [HttpGet("meals/{userId}")]
        public async Task<IActionResult> GetMeals(int userId)
        {
            var meals = await _mealPlansService.GetMealsByUser(userId); // gets meals outside of meal plans
            return Ok(meals);
        }

        [HttpPost("addmealtoexistingmealplan")]
        public async Task<IActionResult> AddMealToExistingPlan([FromBody] Meals meal)
        {
            try
            {
                var addedMeal = await _mealPlansService.AddMealToExistingMealPlan(meal); // adds an independent meal to a meal plan
                return Ok(new { Message = "Meal added to plan", MealId = addedMeal.MealId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to add meal", Error = ex.Message });
            }
        }

        [HttpPost("addusermealplan/{mealPlanId}/{userId}")] // adds mealplanids to a userids account
        public async Task<IActionResult> AddExternalMealPlans(int mealPlanId, int userId)
        {
            try
            {
                var newPlan = await _mealPlansService.AddExternalUserMealPlan(mealPlanId, userId);
                return Ok(new { Message = "Meal Plan added!", MealPlanId = newPlan.MealPlanId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("addusermeal/{mealId}/{userId}")] // adds mealids to a userids account
        public async Task<IActionResult> AddExternalMeal(int mealId, int userId)
        {
            try
            {
                var newMeal = await _mealPlansService.AddExternalUserMeal(mealId, userId);
                return Ok(new { Message = "Meal copied!", MealId = newMeal.MealId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
