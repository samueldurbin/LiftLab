using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; } // table for Users

        public DbSet<FitnessPost> FitnessPosts { get; set; } // table for Fitness related Posts

        public DbSet<Workouts> Workouts { get; set; } // table for Fitness related Posts

        public DbSet<WorkoutPlans> WorkoutPlans { get; set; } // table for Fitness related Posts

        public DbSet<WorkoutPlanData> WorkoutPlansData { get; set; } // table to connect workout plans with workouts in a many-to-many relationship in the database

        public DbSet<Pals> Pals { get; set; }

        public DbSet<FitnessPostComments> FitnessPostComments { get; set; } // Comments for fitness posts



    }
}
