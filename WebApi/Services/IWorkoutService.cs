using Shared.Models;

namespace WebApi.Services
{
    public interface IWorkoutService
    {
        Task<IEnumerable<Workouts>> GetWorkouts();

    }
}
