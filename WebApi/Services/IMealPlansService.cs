using Shared.Models;

namespace WebApi.Services
{
    public interface IMealPlansService
    {
        Task<IEnumerable<MealPlans>> GetAllMealPlans();
        Task<IEnumerable<MealPlans>> GetMealPlansByUser(int userId);
        Task<MealPlans> AddExternalUserMealPlan(int mealPlanId, int userId);
        Task<List<Meals>> GetMealsByPlanId(int planId);
        Task<IEnumerable<Meals>> GetMealsByUserId(int userId);
        Task<Meals> AddMealToExistingMealPlan(Meals meal);
        Task<Meals> AddExternalUserMeal(int mealId, int userId);
        Task<Meals> CreateMeal(Meals meal);
        Task<MealPlans> CreateMealPlan(CreateMealPlanDTO dto);

        Task<bool> DeleteMealPlan(int mealPlanId);
        //Task<bool> DeleteMeal(int mealId);

        Task<bool> DeleteUserMeal(int mealId, int userId);
    }
}
