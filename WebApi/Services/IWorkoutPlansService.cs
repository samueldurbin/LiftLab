using Shared.Models;

namespace WebApi.Services
{
    public interface IWorkoutPlansService
    {      
        Task<IEnumerable<WorkoutPlans>> GetPlans();
        Task<WorkoutPlans> CreatePlan(WorkoutPlans plan, List<int> workoutIds);

    }
}
