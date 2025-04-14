using Shared.Models;

namespace WebApi.Services
{
    public interface ICommunityPostService
    {
        Task<CommunityPost> CreatePost(CommunityPost post); // creates a new user
        Task<IEnumerable<CommunityPost>> GetPosts(); // gets all posts
        Task<IEnumerable<CommunityPostComments>> GetComments(int postId); // gets comments from related posts
        Task<bool> DeleteComment(int commentId);  // deletes comment
        Task<CommunityPostComments> UpdateComments(CommunityPostComments updatedComment); // updates comment
        Task<bool> LikePost(int postId, int userId); // likes a fitness post
        Task<CommunityPostComments> AddComment(CommunityPostComments comment); // adds comments

        Task<IEnumerable<CommunityPost>> GetPostsByUserId(int userId);
    }
}
