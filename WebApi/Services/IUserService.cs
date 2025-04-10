using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Shared.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<Users> LoginAuthentication(string username, string password); // authentication method to compare input and database data
        Task<IEnumerable<Users>> GetUsers(); // gets all users for admin
        Task<Users> CreateUser(Users user);  // creates a new user
        Task<bool> DeleteUser(int userId); // delets user
        Task<Users?> UpdateUser (Users updatedUser); // updates user
        Task<Users> GetUserById(int userId); // gets user by id

    }
}
