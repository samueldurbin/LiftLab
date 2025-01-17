using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Shared.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<UserModel> AuthenticateUser(string username, string password);

        Task<UserModel> GetUsername(string username);  // checks if a username already exists.

        Task<UserModel> RegisterUser(UserModel user);  // creates a new user.

    }
}
