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

        [HttpPost("createmeal")]
        public async Task<IActionResult> CreateMeals([FromBody] Meal meal)
        {
            try
            {
                var createdMeal = await _mealPlansService.CreateMeal(meal); // method from the service to create a meal
                return Ok(new { Message = "Meal has been created successfully", MealId = createdMeal.MealId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to create meal", Error = ex.Message });
            }
        }

        [HttpGet("meals/{userId}")] // endpoint that requires a userid so that it gets the associated meals with that user
        public async Task<IActionResult> GetMeals(int userId)
        {
            var meals = await _mealPlansService.GetMealsByUser(userId); // calls method from service to do a GET request
            return Ok(meals);
        }

        [HttpPost("addmealtoexistingmealplan")]
        public async Task<IActionResult> AddMealToExistingPlan([FromBody] Meal meal)
        {
            try
            {
                var addedMeal = await _mealPlansService.AddMealToExistingMealPlan(meal);
                return Ok(new { Message = "Meal added to plan", MealId = addedMeal.MealId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to add meal", Error = ex.Message });
            }
        }
    }
}
