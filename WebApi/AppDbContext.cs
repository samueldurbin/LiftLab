using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<UserModel> Users { get; set; } // Example table

    }
}
