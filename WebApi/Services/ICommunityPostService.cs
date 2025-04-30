using Shared.Models;

namespace WebApi.Services
{
    public interface ICommunityPostService
    {
        Task<CommunityPost> CreatePost(CommunityPost communityPost); // creates a new user
        Task<IEnumerable<CommunityPost>> GetPosts(); // gets all posts
        Task<IEnumerable<CommunityPostComments>> GetComments(int communityPostId); // gets comments from related posts
        Task<CommunityPostComments> AddComment(CommunityPostComments comment); // adds comments
        Task<bool> DeletePost(int postId, int userId); // deletes post
        Task<IEnumerable<CommunityPost>> GetPostsByUserId(int userId);
    }
}
