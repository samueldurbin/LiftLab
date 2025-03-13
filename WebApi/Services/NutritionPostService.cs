using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi.Services
{
    public class NutritionPostService : INutritionPostService
    {
        private readonly AppDbContext _dbContext; // to use the database

        public NutritionPostService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<NutritionPost> CreatePosts(NutritionPost post) // adds post to the database
        {
            _dbContext.NutritionPosts.Add(post); // adds the post object to the database

            await _dbContext.SaveChangesAsync(); // saves
            return post;
        }

        public async Task<IEnumerable<NutritionPost>> GetPosts() // returns a colletion of nutrition posts
        {
            return await _dbContext.NutritionPosts// retrieves a list of posts from the database
                                 .ToListAsync();
        }
    }
}
