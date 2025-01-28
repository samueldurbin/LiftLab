using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi.Services
{
    public class FitnessPostService : IFitnessPostService
    {
        private readonly AppDbContext _dbContext;

        public FitnessPostService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FitnessPost> CreatePost(FitnessPost post) // adds post to the database
        {
            _dbContext.FitnessPosts.Add(post);
            await _dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<IEnumerable<FitnessPost>> GetPostsAsync() // returns a colletion of fitness posts
        {
            return await _dbContext.FitnessPosts
                                 .OrderByDescending(post => post.CreatedDate) // shows most recent dates first
                                 .ToListAsync();
        }

    }
}
