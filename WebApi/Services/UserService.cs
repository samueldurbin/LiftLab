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

        public async Task<UserModel> ValidateUser(string username, string password)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }
    }
}
