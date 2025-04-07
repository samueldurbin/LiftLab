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
            return await _dbContext.MealPlans.ToListAsync(); // gets all meal plans from the database
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
    }
}
