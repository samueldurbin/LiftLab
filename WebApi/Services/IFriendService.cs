using Shared.Models;

namespace WebApi.Services
{
    public interface IFriendService
    {
        Task<bool> AddFriend(int userId, int friendUserId); // adds friends
        Task<List<int>> GetUsersFriends(int userId); // gets the usersfriends
        Task<List<CommunityPost>> GetFriendsPosts(int userId); // gets the associated fitnessposts
    }
}
