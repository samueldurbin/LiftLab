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

        public async Task<IEnumerable<WorkoutPlans>> GetPlans() // gets all the plans
        {
            return await _dbContext.WorkoutPlans
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkoutPlans>> GetPlansByUser(int userId) // gets the plans assocoiated with the userid
        {
            return await _dbContext.WorkoutPlans // workout plans table
                .Where(u => u.UserId == userId) // userid
                .ToListAsync();
        }

        public async Task<List<int>> GetPlanWorkoutsByPlan(int planId) // gets the workoutids that are in a workoutplan created by a user
        {
            return await _dbContext.WorkoutPlansData // workout plan data table that contains the many-to-many relationship of ids
                .Where(u => u.WorkoutPlanId == planId) // rows match the input planid
                .Select(u => u.WorkoutId) // selects the required ids
                .ToListAsync(); // into a list
        }

        public async Task<WorkoutPlans> CreatePlan(WorkoutPlans plan, List<int> workoutIds) // creates plan from plan name and all the select workouts
        {

            try
            {
                _dbContext.WorkoutPlans.Add(plan); // add the wokrout plan before the data
                await _dbContext.SaveChangesAsync();

                foreach (var workoutId in workoutIds)
                {
                    var workout = await _dbContext.Workouts.AnyAsync(w => w.WorkoutId == workoutId); // checks if workout exists

                    if (!workout) 
                    {
                        throw new Exception("Error in creating a WorkoutPlan"); // error message
                    }

                    _dbContext.WorkoutPlansData.Add(new WorkoutPlanData // adds data and associates workouts with a plan
                    {
                        WorkoutPlanId = plan.WorkoutPlanId, // inputs
                        WorkoutId = workoutId
                    });
                }

                await _dbContext.SaveChangesAsync(); // saves the data

                return plan;
            }
            catch (Exception)
            {
                throw; // just a basic exception for now to see if it works
            }
        }

    }
}
