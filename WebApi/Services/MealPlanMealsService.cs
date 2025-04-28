using Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public class MealPlanMealsService : IMealPlanMealsService
    {
        private readonly AppDbContext _dbContext;

        public MealPlanMealsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Meals>> GetMealsByMealPlanId(int mealPlanId)
        {
            return await _dbContext.MealPlanMeals
                .Where(mpm => mpm.MealPlanId == mealPlanId)
                .Select(mpm => mpm.Meal)
                .ToListAsync();
        }

        public async Task<Meals> CreateMeal(Meals meal)
        {
            _dbContext.Meals.Add(meal);
            await _dbContext.SaveChangesAsync();
            return meal;
        }

        public async Task AddMealToMealPlan(int mealPlanId, int mealId)
        {
            var link = new MealPlanMeals
            {
                MealPlanId = mealPlanId,
                MealId = mealId
            };

            _dbContext.MealPlanMeals.Add(link);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveMealFromMealPlan(int mealPlanId, int mealId)
        {
            var link = await _dbContext.MealPlanMeals
                .FirstOrDefaultAsync(x => x.MealPlanId == mealPlanId && x.MealId == mealId);

            if (link != null)
            {
                _dbContext.MealPlanMeals.Remove(link);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<MealPlans> CreateMealPlanAsync(CreateMealPlanDTO dto)
        {
            var mealPlan = new MealPlans
            {
                UserId = dto.UserId,
                MealPlanName = dto.MealPlanName,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.MealPlans.Add(mealPlan);
            await _dbContext.SaveChangesAsync();

            foreach (var mealId in dto.MealIds)
            {
                var mealPlanMeal = new MealPlanMeals
                {
                    MealPlanId = mealPlan.MealPlanId,
                    MealId = mealId
                };
                _dbContext.MealPlanMeals.Add(mealPlanMeal);
            }

            await _dbContext.SaveChangesAsync();

            return mealPlan;
        }

        public async Task<bool> DeleteMealPlan(int mealPlanId)
        {
            var mealLinks = _dbContext.MealPlanMeals.Where(mpm => mpm.MealPlanId == mealPlanId);
            _dbContext.MealPlanMeals.RemoveRange(mealLinks);

            var mealPlan = await _dbContext.MealPlans.FindAsync(mealPlanId);
            if (mealPlan == null)
            {
                return false;

            }
                
            _dbContext.MealPlans.Remove(mealPlan);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMeal(int mealId)
        {
            var meal = await _dbContext.Meals.FindAsync(mealId);

            if (meal == null)
            {
                return false;

            }
                
            var links = _dbContext.MealPlanMeals.Where(mpm => mpm.MealId == mealId);
            _dbContext.MealPlanMeals.RemoveRange(links);

            _dbContext.Meals.Remove(meal);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Meals> AddExternalUserMeal(int mealId, int userId)
        {

            var originalMeal = await _dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == mealId);

            if (originalMeal == null)
            {
                throw new Exception("Meal not found.");

            }
               

            var copiedMeal = new Meals
            {
                MealName = originalMeal.MealName,
                Type = originalMeal.Type,
                Calories = originalMeal.Calories,
                Recipe = originalMeal.Recipe,
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

                var originalPlan = await _dbContext.MealPlans
                    .Include(mp => mp.MealPlanMeals)
                        .ThenInclude(mpm => mpm.Meal) 
                    .FirstOrDefaultAsync(mp => mp.MealPlanId == mealPlanId);

                if (originalPlan == null)
                    throw new Exception("Original meal plan not found.");

                var copiedPlan = new MealPlans
                {
                    MealPlanName = originalPlan.MealPlanName + " (Copy)",
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.MealPlans.Add(copiedPlan);
                await _dbContext.SaveChangesAsync();

                foreach (var mealPlanMeal in originalPlan.MealPlanMeals)
                {
                    var copiedMeal = new Meals
                    {
                        MealName = mealPlanMeal.Meal.MealName,
                        Type = mealPlanMeal.Meal.Type,
                        Calories = mealPlanMeal.Meal.Calories,
                        Recipe = mealPlanMeal.Meal.Recipe,
                        UserId = userId
                    };

                    _dbContext.Meals.Add(copiedMeal);
                    await _dbContext.SaveChangesAsync();

                    var copiedLink = new MealPlanMeals
                    {
                        MealPlanId = copiedPlan.MealPlanId,
                        MealId = copiedMeal.MealId
                    };

                    _dbContext.MealPlanMeals.Add(copiedLink);
                }

                await _dbContext.SaveChangesAsync();

                return copiedPlan;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while copying Meal Plan to user.", ex);
            }
        }

        public async Task<List<Meals>> GetMealsByUserId(int userId)
        {
            return await _dbContext.Meals
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<MealPlans>> GetMealPlansByUserId(int userId)
        {
            return await _dbContext.MealPlans
                .Where(mp => mp.UserId == userId)
                .ToListAsync();
        }
    }
}
