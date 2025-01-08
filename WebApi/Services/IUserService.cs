using Shared.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<UserModel> ValidateUser(string username, string password);
    }
}
