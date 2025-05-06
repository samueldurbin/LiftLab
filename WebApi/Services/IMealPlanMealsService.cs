using Shared.Models;

namespace WebApi.Services
{
    public interface IMealPlanMealsService
    {
        Task<IEnumerable<Meals>> GetMealsByMealPlanId(int mealPlanId);
        Task RemoveMealFromMealPlan(int mealPlanId, int mealId);
        Task<MealPlans> CreateMealPlan(CreateMealPlanDTO dto);
        Task<bool> DeleteMealPlan(int mealPlanId);
        Task<Meals> AddExternalUserMeal(int mealId, int userId);
        Task<MealPlans> AddExternalUserMealPlan(int mealPlanId, int userId);
        Task<List<Meals>> GetMealsByUserId(int userId);
        Task<List<MealPlans>> GetMealPlansByUserId(int userId);
        Task<Meals> CreateMeal(Meals meal);
        Task<bool> DeleteMeal(int mealId);
    }
}
