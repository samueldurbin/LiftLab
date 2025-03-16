using Shared.Models;

namespace WebApi.Services
{
    public interface IFriendService
    {
        Task<bool> AddFriend(int userId, int friendUserId); // adds users

    }
}
