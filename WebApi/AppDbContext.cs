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

        public DbSet<WMuscleGroups> WMuscleGroups { get; set; }
    }
}
