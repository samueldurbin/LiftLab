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

        public async Task<UserModel> AuthenticateUser(string username, string password)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<UserModel> GetUsername(string username)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<UserModel> RegisterUser(UserModel user)    // adds user to the database
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
