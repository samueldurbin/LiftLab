using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealPlanMealsController : ControllerBase
    {
        private readonly IMealPlanMealsService _mealPlanMealsService;

        public MealPlanMealsController(IMealPlanMealsService service)
        {
            _mealPlanMealsService = service;
        }

        [HttpGet("mealsbyplan/{mealPlanId}")]
        public async Task<IActionResult> GetMealsByMealPlan(int mealPlanId) // gets the meals by plan by the input planid
        {
            var meals = await _mealPlanMealsService.GetMealsByMealPlanId(mealPlanId);

            return Ok(meals);
        }

        [HttpDelete("removemeal/{mealPlanId}/{mealId}")]
        public async Task<IActionResult> RemoveMealFromPlan(int mealPlanId, int mealId) // removes a meal from plan
        {
            await _mealPlanMealsService.RemoveMealFromMealPlan(mealPlanId, mealId);

            return Ok(new { Message = "Meal removed from meal plan." });
        }

        [HttpPost("createmealplan")]
        public async Task<IActionResult> CreateMealPlan([FromBody] CreateMealPlanDTO dto) // creates a meal thats
        {
            var createdPlan = await _mealPlanMealsService.CreateMealPlan(dto);

            return Ok(new { Message = "Meal Plan created", MealPlanId = createdPlan.MealPlanId });
        }

        [HttpDelete("deletemealplan/{mealPlanId}")]
        public async Task<IActionResult> DeleteMealPlan(int mealPlanId) // deletes meal plan
        {
            var result = await _mealPlanMealsService.DeleteMealPlan(mealPlanId);

            if (result)
            {
                return Ok(new { Message = "Meal Plan deleted successfully" });

            }

            return NotFound(new { Message = "Meal Plan not found" });
        }

        [HttpPost("addusermeal/{mealId}/{userId}")]
        public async Task<IActionResult> AddExternalUserMeal(int mealId, int userId) // copies meal from another user to a new user
        {
            try
            {
                var newMeal = await _mealPlanMealsService.AddExternalUserMeal(mealId, userId);

                return Ok(new { Message = "Meal added to user!", MealId = newMeal.MealId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to add meal", Error = ex.Message });
            }
        }

        [HttpPost("addusermealplan/{mealPlanId}/{userId}")]
        public async Task<IActionResult> AddExternalUserMealPlan(int mealPlanId, int userId) // copes meal plan from another user to a new user
        {
            try
            {
                var newPlan = await _mealPlanMealsService.AddExternalUserMealPlan(mealPlanId, userId);

                return Ok(new { Message = "Meal plan copied to user!", MealPlanId = newPlan.MealPlanId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to copy meal plan", Error = ex.Message });
            }
        }

        [HttpGet("meals/user/{userId}")]
        public async Task<IActionResult> GetMealsByUserId(int userId) // get all meals by user id
        {
            var meals = await _mealPlanMealsService.GetMealsByUserId(userId);

            return Ok(meals);
        }

        [HttpGet("mealplans/user/{userId}")]
        public async Task<IActionResult> GetMealPlansByUserId(int userId) // get all meal plans by userid
        {
            var plans = await _mealPlanMealsService.GetMealPlansByUserId(userId);

            return Ok(plans);
        }

        [HttpPost("createmeal")]
        public async Task<IActionResult> CreateMeal([FromBody] Meals meal) // create meal
        {
            var createdMeal = await _mealPlanMealsService.CreateMeal(meal);

            return Ok(createdMeal);
        }

        [HttpDelete("deletemeal/{mealId}")]
        public async Task<IActionResult> DeleteMeal(int mealId) // delete meal
        {
            var deleted = await _mealPlanMealsService.DeleteMeal(mealId);

            if (!deleted)
            {
                return NotFound("Meal not found or could not be deleted.");
            }

            return Ok("Meal deleted successfully!");
        }
    }
}
