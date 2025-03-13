using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi.Services
{
    public class FitnessPostService : IFitnessPostService
    {
        private readonly AppDbContext _dbContext; // to use the database

        public FitnessPostService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FitnessPost> CreatePost(FitnessPost post) // adds post to the database
        {
            _dbContext.FitnessPosts.Add(post); // adds the post object to the database

            await _dbContext.SaveChangesAsync(); // saves
            return post;
        }

        public async Task<IEnumerable<FitnessPost>> GetPosts() // returns a colletion of fitness posts
        {
            return await _dbContext.FitnessPosts // retrieves a list of posts from the database
                                 .ToListAsync();
        }



    }
}
