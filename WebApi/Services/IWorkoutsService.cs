using Shared.Models;

namespace WebApi.Services
{
    public interface IWorkoutsService
    {
        Task<IEnumerable<WorkoutsModel>> GetWorkouts();

    }
}
