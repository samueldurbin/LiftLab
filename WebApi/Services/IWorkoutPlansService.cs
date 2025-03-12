using Shared.Models;

namespace WebApi.Services
{
    public interface IWorkoutPlansService
    {      
        Task<IEnumerable<WorkoutPlans>> GetPlans(); // method to get plans
        Task<WorkoutPlans> CreatePlan(WorkoutPlans plan, List<int> workoutIds); // creates plans with the available workouts

    }
}
