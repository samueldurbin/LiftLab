using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Numerics;

namespace WebApi.Services
{
    public class MealPlansService : IMealPlansService
    {
        private readonly AppDbContext _dbContext;

        public MealPlansService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MealPlan>> GetAllMealPlans()
        {
            return await _dbContext.MealPlans
                .Include(p => p.Meals) // include meals associated with the plan
                .ToListAsync();
        }

        public async Task<IEnumerable<MealPlan>> GetMealPlansByUser(int userId)
        {
            return await _dbContext.MealPlans // get meal plans by userid
                .Where(mp => mp.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Meal>> GetMealsByPlanId(int planId)
        {
            return await _dbContext.Meals
                .Where(m => m.MealPlanId == planId) // get the associated meals within a planid
                .ToListAsync();
        }

        public async Task<MealPlan> CreateMealPlan(MealPlan plan, List<Meal> meals)
        {
            try
            {
                _dbContext.MealPlans.Add(plan);  // add the mealplan
                await _dbContext.SaveChangesAsync();

                foreach (var meal in meals) // add the meals to the mealplan
                {
                    meal.MealPlanId = plan.MealPlanId; 
                    _dbContext.Meals.Add(meal);
                }

                await _dbContext.SaveChangesAsync(); 
                return plan;
            }
            catch (Exception ex)
            {
                throw new Exception("CreateMealPlan has failed: " + ex.InnerException?.Message, ex);
            }
        }

        public async Task<Meal> CreateMeal(Meal meal)
        {
            try
            {
                if (meal.MealPlanId == null && meal.UserId == null)
                {
                    throw new Exception("UserId is required for creating meals.");
                }

                _dbContext.Meals.Add(meal);
                await _dbContext.SaveChangesAsync();

                return meal;
            }
            catch (Exception ex)
            {
                throw new Exception("Creating a Meal has failed: " + ex.InnerException?.Message, ex);
            }
        }

        public async Task<IEnumerable<Meal>> GetMealsByUser(int userId)
        {
            return await _dbContext.Meals
                .Where(m => m.MealPlanId == null && m.UserId == userId)
                .ToListAsync();
        }

        public async Task<Meal> AddMealToExistingMealPlan(Meal meal)
        {
            var existingPlan = await _dbContext.MealPlans.FindAsync(meal.MealPlanId); // check if mealplan exists

            if (existingPlan == null)
            {
                throw new Exception("Meal plan has not been found.");
            }

            _dbContext.Meals.Add(meal);
            await _dbContext.SaveChangesAsync();

            return meal;
        }
    }
}
