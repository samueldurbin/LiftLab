using Microsoft.EntityFrameworkCore;
using Shared.Models;

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
                .Include(p => p.Meals) //  includes the associated meals
                .ToListAsync();

        }

        public async Task<IEnumerable<MealPlan>> GetMealPlansByUser(int userId)
        {
            return await _dbContext.MealPlans
                .Where(mp => mp.UserId == userId) // gets all meal plans created by a userid
                .ToListAsync();
        }

        public async Task<List<Meal>> GetMealsByPlanId(int planId)
        {
            return await _dbContext.Meals
                .Where(m => m.MealPlanId == planId) // gest meals associated with a mealplan
                .ToListAsync();
        }

        public async Task<MealPlan> CreateMealPlan(MealPlan plan, List<Meal> meals)
        {
            try // try catch
            {
                _dbContext.MealPlans.Add(plan); // adds plan to the database
                await _dbContext.SaveChangesAsync(); // this creates the planid which is needed for the meals to be added too

                foreach (var meal in meals)
                {
                    meal.MealPlanId = plan.MealPlanId; // adds mealplanid to a meal
                    _dbContext.Meals.Add(meal); // add meal to database
                }

                await _dbContext.SaveChangesAsync();

                return plan;
            }
            catch (Exception ex) // catch
            {
                throw new Exception("CreateMealPlan has failed: " + ex.InnerException?.Message, ex); // postman and swaggerui debug testing
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
                throw new Exception("Creating a Meal failed: " + ex.InnerException?.Message, ex);
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
            var existingPlan = await _dbContext.MealPlans.FindAsync(meal.MealPlanId);

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
