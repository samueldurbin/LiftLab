using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Users
        public DbSet<Users> Users { get; set; } // table for Users

        // Fitness Posts
        public DbSet<FitnessPost> FitnessPosts { get; set; } // table for Fitness related Posts

        public DbSet<FitnessPostComments> FitnessPostComments { get; set; } // Comments for fitness posts

        public DbSet<FitnessPostLike> FitnessPostLikes { get; set; } // Comments for fitness posts

        // Workouts
        public DbSet<Workouts> Workouts { get; set; } // table for Fitness related Posts

        public DbSet<WorkoutPlans> WorkoutPlans { get; set; } // table for Fitness related Posts

        public DbSet<WorkoutPlanData> WorkoutPlansData { get; set; } // table to connect workout plans with workouts in a many-to-many relationship in the database

        // Nutritional Posts
        public DbSet<NutritionPost> NutritionPosts { get; set; } // Comments for fitness posts

        public DbSet<NutritionPostComments> NutritionPostComments { get; set; } // Comments for fitness posts

        // Friends
        public DbSet<Friend> Friends { get; set; }

    }
}
