using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi.Services
{
    public class WorkoutPlansService : IWorkoutPlansService
    {
        private readonly AppDbContext _dbContext;

        public WorkoutPlansService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<WorkoutPlans>> GetPlans()
        {
            return await _dbContext.WorkoutPlans.ToListAsync();
        }

        public async Task<WorkoutPlans> CreatePlan(WorkoutPlans plan, List<int> workoutIds)
        {

            try
            {
                _dbContext.WorkoutPlans.Add(plan);
                await _dbContext.SaveChangesAsync();

                foreach (var workoutId in workoutIds)
                {
                    var workout = await _dbContext.Workouts.AnyAsync(w => w.WorkoutId == workoutId);
                    if (!workout)
                    {
                        throw new Exception("Error in making WorkoutPlan");
                    }

                    _dbContext.WorkoutPlansData.Add(new WorkoutPlanData
                    {
                        WorkoutPlanId = plan.WorkoutPlanId,
                        WorkoutId = workoutId
                    });
                }

                await _dbContext.SaveChangesAsync();

                return plan;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
