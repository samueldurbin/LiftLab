using Shared.Models;

namespace WebApi.Services
{
    public interface INutritionPostService
    {
        Task<NutritionPost> CreatePosts(NutritionPost post);  // creates a new nutrition post
        Task<IEnumerable<NutritionPost>> GetPosts(); // gets all posts
        Task<NutritionPostComments> AddComment(NutritionPostComments comment); // adds comments
        Task<bool> DeleteComment(int commentId); // deletes comment
        Task<NutritionPostComments> UpdateComments(NutritionPostComments updatedComment); // updates comment
        Task<IEnumerable<NutritionPostComments>> GetComments(int postId); // gets comments from related posts
    }

}
