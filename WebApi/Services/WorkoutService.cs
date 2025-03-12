using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly AppDbContext _dbContext;

        public WorkoutService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Workouts>> GetWorkouts() // returns all of the fitness posts
        {
            return await _dbContext.Workouts // gets all workouts from the databse
                                 .ToListAsync();
        }

    }
}
