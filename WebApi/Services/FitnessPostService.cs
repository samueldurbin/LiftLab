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

        public async Task<FitnessPostComments> AddComment(FitnessPostComments comment)
        { 
            
            comment.Username = "admin"; // make sure hardcoded user is working for testing

            _dbContext.FitnessPostComments.Add(comment); // adds comment to post
            await _dbContext.SaveChangesAsync(); // saves

            return comment;
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            var comment = await _dbContext.FitnessPostComments.FindAsync(commentId); // finds the commentid

            if (comment == null) // checks if the comment exists
            {
                return false;
            }

            _dbContext.FitnessPostComments.Remove(comment); // deletes the comment in the database from the id input
            await _dbContext.SaveChangesAsync();

            return true;

        }



    }

}

