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

        public async Task<NutritionPostComments> AddComment(NutritionPostComments comment)
        {

            comment.Username = "admin"; // make sure hardcoded user is working for testing

            _dbContext.NutritionPostComments.Add(comment); // adds comment to post
            await _dbContext.SaveChangesAsync(); // saves

            return comment;
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            var comment = await _dbContext.NutritionPostComments.FindAsync(commentId); // finds the commentid

            if (comment == null) // checks if the comment exists
            {
                return false;
            }

            _dbContext.FitnessPostComments.Remove(comment); // deletes the comment in the database from the id input
            await _dbContext.SaveChangesAsync();

            return true;

        }

        public async Task<NutritionPostComments> UpdateComments(NutritionPostComments updatedComment) // updates the comment
        {
            var comment = await _dbContext.NutritionPostComments.FindAsync(updatedComment.NutritionPostCommentId); // searches the fitnesspostcommentid

            if (comment == null) // checks to see if comment exists and returns null if it doesnt
            {
                return null;
            }

            comment.Comment = updatedComment.Comment; // updates user data with the input

            await _dbContext.SaveChangesAsync();

            return comment;

        }
    }
}
