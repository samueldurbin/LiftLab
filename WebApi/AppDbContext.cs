using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; } // table for Users

        public DbSet<FitnessPost> FitnessPosts { get; set; } // table for Fitness related Posts

        public DbSet<WorkoutsModel> Workouts { get; set; } // table for Fitness related Posts

    }
}
