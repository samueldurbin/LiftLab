using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; } // table for Users

        public DbSet<Workouts> Workouts { get; set; } // table for Fitness related Posts

        public DbSet<WorkoutPlans> WorkoutPlans { get; set; } // table for Fitness related Posts

        public DbSet<WorkoutPlanData> WorkoutPlansData { get; set; } // table to connect workout plans with workouts in a many-to-many relationship in the database

        public DbSet<Friend> Friends { get; set; }

        public DbSet<CommunityPost> CommunityPosts { get; set; } // table for Fitness related Posts

        public DbSet<CommunityPostComments> CommunityPostComments { get; set; } // Comments for fitness posts

        public DbSet<CommunityPostLike> CommunityPostLikes { get; set; } // Comments for fitness posts

        public DbSet<MealPlans> MealPlans { get; set; } // Meal Plan

        public DbSet<Meals> Meals { get; set; } // Meals within a Meal Plan
        public DbSet<MealPlanMeals> MealPlanMeals { get; set; }
    }
}
