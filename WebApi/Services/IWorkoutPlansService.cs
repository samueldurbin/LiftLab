using Shared.Models;

namespace WebApi.Services
{
    public interface IWorkoutPlansService
    {      
        Task<IEnumerable<WorkoutPlans>> GetPlans(); // method to get plans
        Task<WorkoutPlans> CreatePlan(WorkoutPlans plan, List<int> workoutIds); // creates plans with the available workouts
        Task<IEnumerable<WorkoutPlans>> GetPlansByUser(int userId); // method to get plans by userid
        Task<List<int>> GetPlanWorkoutsByPlan(int planId); // this gets the workout ids in a list by planid
        Task<WorkoutPlans> AddExternalUserWorkoutPlan(int planId, int userId);

    }
}
