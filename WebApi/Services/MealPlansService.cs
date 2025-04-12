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

        public async Task<IEnumerable<MealPlans>> GetAllMealPlans()
        {
            return await _dbContext.MealPlans
                .Include(p => p.Meals) // include meals associated with the plan
                .ToListAsync();
        }

        public async Task<IEnumerable<MealPlans>> GetMealPlansByUser(int userId)
        {
            return await _dbContext.MealPlans // get meal plans by userid
                .Where(mp => mp.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Meals>> GetMealsByPlanId(int planId)
        {
            return await _dbContext.Meals
                .Where(m => m.MealPlanId == planId) // get the associated meals within a planid
                .ToListAsync();
        }

        public async Task<MealPlans> CreateMealPlan(MealPlans plan, List<Meals> meals)
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

        public async Task<Meals> CreateMeal(Meals meal)
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

        public async Task<IEnumerable<Meals>> GetMealsByUser(int userId)
        {
            return await _dbContext.Meals
                .Where(m => m.MealPlanId == null && m.UserId == userId)
                .ToListAsync();
        }

        public async Task<Meals> AddMealToExistingMealPlan(Meals meal)
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

        public async Task<Meals> AddExternalUserMeal(int mealId, int userId)
        {
            var meal = await _dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == mealId);

            if (meal == null)
                throw new Exception("Meal not found.");

            var copiedMeal = new Meals
            {
                MealName = meal.MealName,
                Type = meal.Type,
                Calories = meal.Calories,
                Recipe = meal.Recipe,
                UserId = userId
            };

            _dbContext.Meals.Add(copiedMeal);
            await _dbContext.SaveChangesAsync();

            return copiedMeal;
        }

        public async Task<MealPlans> AddExternalUserMealPlan(int mealPlanId, int userId)
        {
            try
            {
                var mealPlan = await _dbContext.MealPlans
                    .Include(p => p.Meals)
                    .FirstOrDefaultAsync(p => p.MealPlanId == mealPlanId);

                if (mealPlan == null)
                {
                    throw new Exception("Meal plan not found.");
                }

                var userPlan = new MealPlans
                {
                    MealPlanName = mealPlan.MealPlanName + " (Added)",
                    UserId = userId
                };

                _dbContext.MealPlans.Add(userPlan);
                await _dbContext.SaveChangesAsync();

                foreach (var item in mealPlan.Meals)
                {
                    var copiedMeal = new Meals
                    {
                        MealPlanId = userPlan.MealPlanId,
                        MealName = item.MealName,
                        Type = item.Type,
                        Calories = item.Calories,
                        Recipe = item.Recipe,
                        UserId = userId
                    };

                    _dbContext.Meals.Add(copiedMeal);
                }

                await _dbContext.SaveChangesAsync();
                return userPlan;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding the meal plan to your account!", ex);
            }
        }
    }
}
