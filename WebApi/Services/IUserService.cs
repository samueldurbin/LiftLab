using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Shared.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<Users> LoginAuthentication(string username, string password);
        Task<IEnumerable<Users>> GetUsers(); // gets all users for admin
        Task<Users> CreateUser(Users user);  // creates a new user
        Task<bool> DeleteUser(int userId);

    }
}
