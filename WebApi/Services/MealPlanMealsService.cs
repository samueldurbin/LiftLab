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
            return await _dbContext.MealPlanMeals // get the many to many relationship table
                .Where(mpm => mpm.MealPlanId == mealPlanId)  // filter it by the planid
                .Select(mpm => mpm.Meal) // gets the related meal
                .ToListAsync(); // returns them as a lsit
        }

        public async Task<Meals> CreateMeal(Meals meal)
        {
            _dbContext.Meals.Add(meal); // add meal
            await _dbContext.SaveChangesAsync(); // save to database
            return meal;
        }

        public async Task RemoveMealFromMealPlan(int mealPlanId, int mealId)
        {
            var meal = await _dbContext.MealPlanMeals
                .FirstOrDefaultAsync(x => x.MealPlanId == mealPlanId && x.MealId == mealId); // the link between mealplanid and mealid

            if (meal != null) // if meal was found in the meal plan, remove it
            {
                _dbContext.MealPlanMeals.Remove(meal);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<MealPlans> CreateMealPlan(CreateMealPlanDTO dto)
        {
            var mealPlan = new MealPlans // create a new meal plan from te input data
            {
                UserId = dto.UserId, // the userid who makes it
                MealPlanName = dto.MealPlanName, // the new meal plan name
                CreatedAt = DateTime.UtcNow // the date it was created
            };

            _dbContext.MealPlans.Add(mealPlan); // add the meal plan
            await _dbContext.SaveChangesAsync();

            foreach (var mealId in dto.MealIds) // add the meals to the new plan
            {
                var mealPlanMeal = new MealPlanMeals
                {
                    MealPlanId = mealPlan.MealPlanId,
                    MealId = mealId
                };

                _dbContext.MealPlanMeals.Add(mealPlanMeal); // save
            }

            await _dbContext.SaveChangesAsync();

            return mealPlan; // return the created plan
        }

        public async Task<bool> DeleteMealPlan(int mealPlanId) // deletes meal plan and all assoicated meals
        {
            var meals = _dbContext.MealPlanMeals.Where(mpm => mpm.MealPlanId == mealPlanId); // gets the links between the plan and meals
            _dbContext.MealPlanMeals.RemoveRange(meals); // removes the links

            var mealPlan = await _dbContext.MealPlans.FindAsync(mealPlanId); // find the mealplan itself

            if (mealPlan == null)
            {
                return false;

            }
                
            _dbContext.MealPlans.Remove(mealPlan); // delete from database

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteMeal(int mealId) // deletes meal and any links to the meal plans
        {
            var meal = await _dbContext.Meals.FindAsync(mealId); // find the mealid

            if (meal == null)
            {
                return false;
            }

            var mealLinks = _dbContext.MealPlanMeals.Where(m => m.MealId == mealId);
            _dbContext.MealPlanMeals.RemoveRange(mealLinks); // removes the meal links

            _dbContext.Meals.Remove(meal); // deletes meal

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<Meals> AddExternalUserMeal(int mealId, int userId)
        {

            var originalMeal = await _dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == mealId); // finds the original meal created by another user

            if (originalMeal == null) // checks if the meal actually exists
            {
                throw new Exception("Meal not found.");
            }

            var copiedMeal = new Meals // create a new meal with the same properties but for a new user
            {
                MealName = originalMeal.MealName,
                Type = originalMeal.Type,
                Calories = originalMeal.Calories,
                Recipe = originalMeal.Recipe,
                UserId = userId 
            };

            _dbContext.Meals.Add(copiedMeal); // add copied meal
            await _dbContext.SaveChangesAsync();

            return copiedMeal;
        }

        public async Task<MealPlans> AddExternalUserMealPlan(int mealPlanId, int userId)
        {
            try
            { // retreive the original meal plan, including its meals 

                var originalPlan = await _dbContext.MealPlans
                    .Include(mp => mp.MealPlanMeals)
                    .ThenInclude(mpm => mpm.Meal)
                    .FirstOrDefaultAsync(mp => mp.MealPlanId == mealPlanId);

                if (originalPlan == null) // if not found, throw an exception
                {
                    throw new Exception("Original meal plan not found.");
                }

                var copiedPlan = new MealPlans // create a new meal plans object for the new user
                {
                    MealPlanName = originalPlan.MealPlanName + " (Added)", // this adds an added word infront of the mealplan to differentiate it
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.MealPlans.Add(copiedPlan); // generate a new meal planid
                await _dbContext.SaveChangesAsync();

                foreach (var mealPlanMeal in originalPlan.MealPlanMeals)
                {
                    var copiedMeal = new Meals // create a new meal object
                    {
                        MealName = mealPlanMeal.Meal.MealName, // copy meal name
                        Type = mealPlanMeal.Meal.Type, // copy meal type
                        Calories = mealPlanMeal.Meal.Calories, // copy meal calories
                        Recipe = mealPlanMeal.Meal.Recipe, // copy meal recipe
                        UserId = userId // assign to new user
                    };

                    _dbContext.Meals.Add(copiedMeal); // add the copied meal
                    await _dbContext.SaveChangesAsync();

                    var copiedLink = new MealPlanMeals // create a new link between the copied meal and the new copied plan
                    {
                        MealPlanId = copiedPlan.MealPlanId,
                        MealId = copiedMeal.MealId
                    };

                    _dbContext.MealPlanMeals.Add(copiedLink); // adds the meals within a mealplan link
                }

                await _dbContext.SaveChangesAsync();

                return copiedPlan;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while adding Meal Plan.", ex);
            }
        }

        public async Task<List<Meals>> GetMealsByUserId(int userId)
        {
            return await _dbContext.Meals
                .Where(m => m.UserId == userId) // get meals associated by the userid
                .ToListAsync();
        }

        public async Task<List<MealPlans>> GetMealPlansByUserId(int userId)
        {
            return await _dbContext.MealPlans
                .Where(mp => mp.UserId == userId) // get meal plans associated by userid
                .ToListAsync();
        }
    }
}
