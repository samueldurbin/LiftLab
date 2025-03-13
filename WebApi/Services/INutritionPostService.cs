using Shared.Models;

namespace WebApi.Services
{
    public interface INutritionPostService
    {
        Task<NutritionPost> CreatePosts(NutritionPost post);  // creates a new nutrition post
        Task<IEnumerable<NutritionPost>> GetPosts(); // gets all posts
    }
}
