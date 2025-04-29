using Shared.Models;

namespace WebApi.Services
{
    public interface IMealPlanMealsService
    {
        Task<IEnumerable<Meals>> GetMealsByMealPlanId(int mealPlanId);
        Task AddMealToMealPlan(int mealPlanId, int mealId);
        Task RemoveMealFromMealPlan(int mealPlanId, int mealId);
        Task<MealPlans> CreateMealPlanAsync(CreateMealPlanDTO dto);
        Task<bool> DeleteMealPlan(int mealPlanId);
        //Task<bool> DeleteMeal(int mealId);
        Task<Meals> AddExternalUserMeal(int mealId, int userId);
        Task<MealPlans> AddExternalUserMealPlan(int mealPlanId, int userId);
        Task<List<Meals>> GetMealsByUserId(int userId);
        Task<List<MealPlans>> GetMealPlansByUserId(int userId);
        Task<Meals> CreateMeal(Meals meal);
        Task<List<MealPlans>> GetMealPlansByMealId(int mealId);
        Task<bool> DeleteMeal(int mealId);
    }
}
