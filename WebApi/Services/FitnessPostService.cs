using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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

        public async Task<IEnumerable<FitnessPostComments>> GetComments(int postId) // returns fitness post related comments
        {
            return await _dbContext.FitnessPostComments // table of comments in the database
                .Where(c => c.FitnessPostId == postId) // gets comments that are linked to a specific psot
                .ToListAsync();  // gets list
        }

        // compared to the previous method, this one checks if the post exists, increments a comment counter needed for front end
        // and is better for data integrity
        public async Task<FitnessPostComments> AddComment(FitnessPostComments comment)
        {
            var post = await _dbContext.FitnessPosts.FindAsync(comment.FitnessPostId); // finds the fitnesspost from the database using the fitnesspostid

            if (post == null)
            {
                throw new Exception("Fitness post has not been found"); // this is mainly for backend testing as a fitness post that did not exist would not show in the front end
            }

            _dbContext.FitnessPostComments.Add(comment); // adds the comment to the fitnesspost

            post.CommentCount += 1; // this increments the comment counter in the database

            await _dbContext.SaveChangesAsync(); // saves both the comment and the counter

            return comment;
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            var comment = await _dbContext.FitnessPostComments.FindAsync(commentId); // finds the commentid in the database

            if (comment == null) // checks if the comment exists
            {
                return false; // return false if the comment does ot exist
            }

            var post = await _dbContext.FitnessPosts.FindAsync(comment.FitnessPostId); // finds the related fitnesspost

            if (post != null && post.CommentCount > 0)
            {
                post.CommentCount -= 1; // if a comment is deleted the comment counter would decrease
            }

            _dbContext.FitnessPostComments.Remove(comment); // deletes the comment in the database from the id input
            await _dbContext.SaveChangesAsync();

            return true;

        }

        public async Task<FitnessPostComments> UpdateComments(FitnessPostComments updatedComment) // updates the comment
        {
            var comment = await _dbContext.FitnessPostComments.FindAsync(updatedComment.FitnessPostCommentId); // searches the fitnesspostcommentid

            if (comment == null) // checks to see if comment exists and returns null if it doesnt
            {
                return null;
            }

            comment.Comment = updatedComment.Comment; // updates user data with the input

            await _dbContext.SaveChangesAsync();

            return comment;

        }

        public async Task<bool> LikePost(int postId, int userId) // adds a like to a  fitnesspost
        {
            var post = await _dbContext.FitnessPosts.FindAsync(postId); // finds a fitnesspost in the database or one that will be shown in the frontend

            if (post == null)  // checks if the post exists which is mainly for backend testing as posts that dont exist wont be shown in the front end
            {
                return false; // returns false if the post does not exist
            }

            var alreadyLiked = await _dbContext.FitnessPostLikes
                .AnyAsync(l => l.FitnessPostId == postId && l.UserId == userId); // this prevents a post from being liked more than once

            if (alreadyLiked) // this will check if a post has already been liked and return false (again mainly for backend testing)
            {
                return false;
            }

            _dbContext.FitnessPostLikes.Add(new FitnessPostLike // adds a new entry into the database if the psot is liked
            {
                FitnessPostId = postId, // the fitness post being liked by a user
                UserId = userId // the userid liking the post
            });

            post.LikeCount += 1; // increments the like count by 1 if the post is liked by a user

            await _dbContext.SaveChangesAsync(); // saves c

            return true;
        }

    }

}

