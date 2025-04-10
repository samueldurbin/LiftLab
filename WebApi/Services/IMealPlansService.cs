using Shared.Models;

namespace WebApi.Services
{
    public interface IMealPlansService
    {
        Task<IEnumerable<MealPlans>> GetAllMealPlans();
        Task<IEnumerable<MealPlans>> GetMealPlansByUser(int userId);
        Task<List<Meal>> GetMealsByPlanId(int planId);
        Task<MealPlans> CreateMealPlan(MealPlans plan, List<Meal> meals);
        Task<Meal> CreateMeal(Meal meal);
        Task<IEnumerable<Meal>> GetMealsByUser(int userId);
        Task<Meal> AddMealToExistingMealPlan(Meal meal);

    }
}
