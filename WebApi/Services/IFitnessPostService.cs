using Microsoft.Extensions.Configuration.UserSecrets;
using Shared.Models;

namespace WebApi.Services
{
    public interface IFitnessPostService
    {
        // Posts
        Task<FitnessPost> CreatePost(FitnessPost post);  // creates a new user
        Task<IEnumerable<FitnessPost>> GetPosts(); // gets all posts

        // Comments
        //Task<FitnessPostComments> AddComment(FitnessPostComments comment); // adds comments
        Task<bool> DeleteComment(int commentId); // deletes comment
        Task<FitnessPostComments> UpdateComments(FitnessPostComments updatedComment); // updates comment
        Task<IEnumerable<FitnessPostComments>> GetComments(int postId); // gets comments from related posts

        // Likes
        Task<bool> LikePost(int postId, int userId); // likes a fitness post

    }
}
