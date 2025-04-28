using Microsoft.AspNetCore.Mvc;
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

        //public async Task<IEnumerable<MealPlans>> GetAllMealPlans()
        //{
        //    return await _dbContext.MealPlans
        //        .Include(p => p.Meals)
        //        .ToListAsync();
        //}

        //public async Task<IEnumerable<MealPlans>> GetMealPlansByUser(int userId)
        //{
        //    return await _dbContext.MealPlans
        //        .Include(mp => mp.Meals)
        //        .Where(mp => mp.UserId == userId)
        //        .ToListAsync();
        //}

        //public async Task<List<Meals>> GetMealsByPlanId(int planId)
        //{
        //    return await _dbContext.Meals
        //        .Where(m => m.MealPlanId == planId)
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<Meals>> GetMealsByUserId(int userId)
        {
            return await _dbContext.Meals
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }

        public async Task<MealPlans> CreateMealPlan(CreateMealPlanDTO dto)
        {
            var mealPlan = new MealPlans // creates a new MealPlan using the Data Transfer Object
            {
                MealPlanName = dto.MealPlanName, // this is for the user to create a meal plan name
                UserId = dto.UserId, // this is the userid that creates the workoutplan
                CreatedAt = DateTime.UtcNow // date and time it was created
            };

            _dbContext.MealPlans.Add(mealPlan); // adds meal plan to the database
            await _dbContext.SaveChangesAsync();
             
            var meals = await _dbContext.Meals // this gets all the meals created by the user
                .Where(m => dto.MealIds.Contains(m.MealId)) // this is a list of ids that will link to the meals database
                .ToListAsync();

            //foreach (var meal in meals) // this sets the planid for the meals
            //{
            //    meal.MealPlanId = mealPlan.MealPlanId;
            //}

            await _dbContext.SaveChangesAsync();
            return mealPlan;
        }

        public async Task<Meals> CreateMeal(Meals meal)
        {
            _dbContext.Meals.Add(meal);
            await _dbContext.SaveChangesAsync();
            return meal;
        }


        //public async Task<Meals> AddMealToExistingMealPlan(Meals meal)
        //{
        //    var existingPlan = await _dbContext.MealPlans.FindAsync(meal.MealPlanId); // check if mealplan exists

        //    if (existingPlan == null)
        //    {
        //        throw new Exception("Meal plan has not been found.");
        //    }

        //    _dbContext.Meals.Add(meal);
        //    await _dbContext.SaveChangesAsync();

        //    return meal;
        //}

        //public async Task<Meals> AddExternalUserMeal(int mealId, int userId)
        //{
        //    var meal = await _dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == mealId);

        //    if (meal == null)
        //        throw new Exception("Meal not found.");

        //    var copiedMeal = new Meals
        //    {
        //        MealName = meal.MealName,
        //        Type = meal.Type,
        //        Calories = meal.Calories,
        //        Recipe = meal.Recipe,
        //        UserId = userId
        //    };

        //    _dbContext.Meals.Add(copiedMeal);
        //    await _dbContext.SaveChangesAsync();

        //    return copiedMeal;
        //}

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

        public async Task<bool> DeleteMealPlan(int mealPlanId)
        {
            var mealPlan = await _dbContext.MealPlans
                .Include(mp => mp.Meals)
                .FirstOrDefaultAsync(mp => mp.MealPlanId == mealPlanId);

            if (mealPlan == null) return false;

            _dbContext.MealPlans.Remove(mealPlan); // Delete the meal plan

            // Optionally, delete the meals associated with the plan
            _dbContext.Meals.RemoveRange(mealPlan.Meals);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        // Delete a meal by ID
        //public async Task<bool> DeleteMeal(int mealId)
        //{
        //    var meal = await _dbContext.Meals
        //        .Include(m => m.MealPlan)
        //        .FirstOrDefaultAsync(m => m.MealId == mealId);

        //    if (meal == null)
        //        return false;

        //    // Remove the meal from the meal plan
        //    if (meal.MealPlan != null)
        //    {
        //        meal.MealPlan.Meals.Remove(meal);
        //    }

        //    _dbContext.Meals.Remove(meal); // Delete the meal
        //    await _dbContext.SaveChangesAsync();  // Save changes to the database

        //    return true;
        //}

        public async Task<Meals> AddExternalUserMeal(int mealId, int userId)
        {
            // Fetch the original meal
            var meal = await _dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == mealId);

            if (meal == null)
                throw new Exception("Meal not found.");

            // Create a new meal based on the original meal
            var copiedMeal = new Meals
            {
                MealName = meal.MealName,
                Type = meal.Type,
                Calories = meal.Calories,
                Recipe = meal.Recipe,
                UserId = userId  // Make sure to associate it with the user's account
            };

            // Add the copied meal to the database
            _dbContext.Meals.Add(copiedMeal);
            await _dbContext.SaveChangesAsync();

            return copiedMeal;  // Return the new meal (user's copy)
        }

        public async Task<bool> DeleteUserMeal(int mealId, int userId)
        {
            var meal = await _dbContext.Meals
                .FirstOrDefaultAsync(m => m.MealId == mealId && m.UserId == userId); // Find the meal by UserId

            if (meal == null)
                return false;  // Meal not found for this user

            _dbContext.Meals.Remove(meal);  // Remove the user's specific meal
            await _dbContext.SaveChangesAsync();

            return true;  // Successfully deleted
        }
    }
}
