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

        public async Task<WorkoutPlans> AddExternalUserWorkoutPlan(int planId, int userId)
        {
            try
            { 
                var workoutPlan = await _dbContext.WorkoutPlans // finds the workout plan id to be added by a user
                    .Include(p => p.WorkoutPlansData) // incldues the associated data with the workoutplan id
                    .FirstOrDefaultAsync(p => p.WorkoutPlanId == planId);

                if (workoutPlan == null) // if the workout plan doesnt exist, throw an error. however this is mainly for backend testing as a plan wouldnt be presnt in ui if didnt exist.
                {
                    throw new Exception("Original workout plan not found.");
                }

                var userPlan = new WorkoutPlans // create a new workoutplan with the name and a added message to test for frontend
                {
                    WorkoutPlanName = workoutPlan.WorkoutPlanName + " (Added)",
                    UserId = userId // workout plan to the userid
                };

                _dbContext.WorkoutPlans.Add(userPlan); // adds new workout plan to the database

                await _dbContext.SaveChangesAsync(); // saves changes

                var workouts = await _dbContext.WorkoutPlansData
                    .Where(w => w.WorkoutPlanId == planId)
                    .ToListAsync();

                foreach (var item in workouts) // adding workouts to the new workout plan that was just created
                {
                    _dbContext.WorkoutPlansData.Add(new WorkoutPlanData // adds copied workouts to the workoutplan
                    {
                        WorkoutPlanId = userPlan.WorkoutPlanId, // new planid
                        WorkoutId = item.WorkoutId // use same workout ids as original
                    });
                }

                await _dbContext.SaveChangesAsync(); // saves changes

                return userPlan; // returns new workoutplan
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding the workout plan to your account!", ex);
            }
        }

        public async Task<bool> AddWorkoutToPlan(int workoutPlanId, int workoutId)
        {
            var workoutPlanExists = await _dbContext.WorkoutPlans.AnyAsync(p => p.WorkoutPlanId == workoutPlanId); // this checks if the workoutplan exists

            var workoutExists = await _dbContext.Workouts.AnyAsync(w => w.WorkoutId == workoutId); // this checks if the workout exists

            if (!workoutPlanExists || !workoutExists) // checks if both the workoutplan and the workout that is going to be added exists first to prevent errors
            {
                return false;
            }

            var alreadyAdded = await _dbContext.WorkoutPlansData
                .AnyAsync(w => w.WorkoutPlanId == workoutPlanId && w.WorkoutId == workoutId); // this checks the workoutplansdata table to see if the workout already exists witin the plan to prevent duplicate workouts

            if (alreadyAdded) // this prevents duplicates
            {
                return false;
            }

            _dbContext.WorkoutPlansData.Add(new WorkoutPlanData // this essentially adds the new workoutid to the plan
            {
                WorkoutPlanId = workoutPlanId,
                WorkoutId = workoutId
            });

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteWorkoutFromPlan(int workoutPlanId, int workoutId)
        {
            var input = await _dbContext.WorkoutPlansData
                .FirstOrDefaultAsync(w => w.WorkoutPlanId == workoutPlanId && w.WorkoutId == workoutId); // this i used to find the workoutplanId and the selected workoutId within the workoutplandata tobe deleted

            if (input == null) // return false if the workoutid does not exist
            {
                return false;
            }

            _dbContext.WorkoutPlansData.Remove(input); // this removes the record from the workoutplansdata table
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
