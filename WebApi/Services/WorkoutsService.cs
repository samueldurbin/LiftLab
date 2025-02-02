
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi.Services
{
    public class WorkoutsService : IWorkoutsService
    {
        private readonly AppDbContext _dbContext;

        public WorkoutsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<WorkoutsModel>> GetWorkouts() // returns a colletion of fitness posts
        {
            return await _dbContext.Workouts
                                 .ToListAsync();
        }

    }
}
