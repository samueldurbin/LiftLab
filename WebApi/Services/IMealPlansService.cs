using Shared.Models;

namespace WebApi.Services
{
    public interface IMealPlansService
    {
        Task<IEnumerable<MealPlan>> GetAllMealPlans();
        Task<IEnumerable<MealPlan>> GetMealPlansByUser(int userId);
        Task<List<Meal>> GetMealsByPlanId(int planId);
        Task<MealPlan> CreateMealPlan(MealPlan plan, List<Meal> meals);
        Task<Meal> CreateMeal(Meal meal);
        Task<IEnumerable<Meal>> GetMealsByUser(int userId);
        Task<Meal> AddMealToExistingMealPlan(Meal meal);

    }
}
