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

        public async Task<WorkoutPlans> CreatePlan(WorkoutPlans plan, List<WorkoutInPlanDTO> workouts)
        {
            _dbContext.WorkoutPlans.Add(plan); // this adds the workout plan to the database
            await _dbContext.SaveChangesAsync();

            foreach (var workout in workouts) // this loops through each workout beimg added
            {
                var workoutExists = await _dbContext.Workouts.AnyAsync(w => w.WorkoutId == workout.WorkoutId); // checks if the workout actually exists

                if (!workoutExists)
                {
                    throw new Exception($"Workout with Id {workout.WorkoutId} has nt been found!");
                }

                _dbContext.WorkoutPlansData.Add(new WorkoutPlanData // this adds the workouts, reps and sets to the workoutplan that was just created
                {
                    WorkoutPlanId = plan.WorkoutPlanId,
                    WorkoutId = workout.WorkoutId,
                    Reps = workout.Reps,
                    Sets = workout.Sets
                });
            }

            await _dbContext.SaveChangesAsync();
            return plan;
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

        public async Task<bool> AddWorkoutToPlan(int workoutPlanId, int workoutId, int reps, int sets)
        {
            var workoutPlanExists = await _dbContext.WorkoutPlans.AnyAsync(p => p.WorkoutPlanId == workoutPlanId);  // this checks if the workoutplan exists
            var workoutExists = await _dbContext.Workouts.AnyAsync(w => w.WorkoutId == workoutId); // this checks if the workout exists

            if (!workoutPlanExists || !workoutExists) // checks if both the workoutplan and the workout that is going to be added exists first to prevent errors
                return false;

            var alreadyAdded = await _dbContext.WorkoutPlansData
                .AnyAsync(w => w.WorkoutPlanId == workoutPlanId && w.WorkoutId == workoutId); // this checks the workoutplansdata table to see if the workout already exists witin the plan to prevent duplicate workouts

            if (alreadyAdded) // this prevents duplicates
                return false;

            _dbContext.WorkoutPlansData.Add(new WorkoutPlanData // this essentially adds the new workoutid to the plan
            {
                WorkoutPlanId = workoutPlanId,
                WorkoutId = workoutId,
                Reps = reps,
                Sets = sets
            });

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<WorkoutInPlanDTO>> GetWorkoutPlanDetails(int planId)
        {
            return await _dbContext.WorkoutPlansData
                .Where(w => w.WorkoutPlanId == planId)
                .Select(w => new WorkoutInPlanDTO
                {
                    WorkoutId = w.WorkoutId,
                    Reps = w.Reps,
                    Sets = w.Sets,
                    Kg = w.Kg
                })
                .ToListAsync();
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

        public async Task<bool> DeleteWorkoutPlan(int planId)
        {
            var hasPosts = await _dbContext.CommunityPosts
                .AnyAsync(p => p.WorkoutPlanId == planId);

            if (hasPosts)
            {
                throw new InvalidOperationException("This workout plan is linked to community posts and cannot be deleted.");
            }

            var plan = await _dbContext.WorkoutPlans.FindAsync(planId);
            if (plan == null) return false;

            var relatedWorkouts = _dbContext.WorkoutPlansData
                .Where(w => w.WorkoutPlanId == planId);
            _dbContext.WorkoutPlansData.RemoveRange(relatedWorkouts);

            _dbContext.WorkoutPlans.Remove(plan);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateWorkoutInPlan(UpdateWorkoutInPlanDTO dto)
        {
            var workoutPlanData = await _dbContext.WorkoutPlansData
                .FirstOrDefaultAsync(w => w.WorkoutPlanId == dto.WorkoutPlanId && w.WorkoutId == dto.WorkoutId);

            if (workoutPlanData == null)
            {
                return false;
            }

            workoutPlanData.Reps = dto.Reps;
            workoutPlanData.Sets = dto.Sets;
            workoutPlanData.Kg = dto.Kg;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
