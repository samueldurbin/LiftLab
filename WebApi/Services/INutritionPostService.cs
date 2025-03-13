using Shared.Models;

namespace WebApi.Services
{
    public interface INutritionPostService
    {
        Task<NutritionPost> CreatePosts(NutritionPost post);  // creates a new user
        Task<IEnumerable<NutritionPost>> GetPosts(); // gets all posts
    }
}
