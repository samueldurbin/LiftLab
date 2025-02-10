using Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Users> LoginAuthentication(string username, string password)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<IEnumerable<Users>> GetUsers() // returns a colletion of fitness posts
        {
            return await _dbContext.Users
                                 .ToListAsync();
        }

        public async Task<Users> CreateUser(Users user) // adds post to the database
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
