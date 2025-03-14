using Shared.Models;

namespace WebApi.Services
{
    public interface IFitnessPostService
    {
        Task<FitnessPost> CreatePost(FitnessPost post);  // creates a new user
        Task<IEnumerable<FitnessPost>> GetPosts(); // gets all posts
        Task<FitnessPostComments> AddComment(FitnessPostComments comment); // adds comments
        Task<bool> DeleteComment(int commentId); // deletes comment
        Task<FitnessPostComments> UpdateComments(FitnessPostComments updatedComment); // updates comment
        Task<IEnumerable<FitnessPostComments>> GetComments(int postId); // gets comments from related posts

    }
}
