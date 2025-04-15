using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi.Services
{
    public class CommunityPostService : ICommunityPostService
    {
        private readonly AppDbContext _dbContext; // to use the database

        public CommunityPostService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommunityPost> CreatePost(CommunityPost post) // adds post to the database
        {
            _dbContext.CommunityPosts.Add(post); // adds the post object to the database

            await _dbContext.SaveChangesAsync(); // saves
            return post;
        }

        public async Task<IEnumerable<CommunityPost>> GetPosts() // returns a colletion of fitness posts
        {
            return await _dbContext.CommunityPosts.ToListAsync(); // retrieves a list of posts from the database
        }

        // compared to the previous method, this one checks if the post exists, increments a comment counter needed for front end
        // and is better for data integrity
        public async Task<CommunityPostComments> AddComment(CommunityPostComments comment) 
        {
            var post = await _dbContext.CommunityPosts.FindAsync(comment.CommunityPostId); // finds the post from the database using the postid

            if (post == null)
            {
                throw new Exception("Fitness post has not been found"); // this is mainly for backend testing as a post that did not exist would not show in the front end
            }

            _dbContext.CommunityPostComments.Add(comment); // adds the comment to the post

            post.CommentCount += 1; // this increments the comment counter in the database

            await _dbContext.SaveChangesAsync(); // saves both the comment and the counter
            return comment;
        }

        public async Task<IEnumerable<CommunityPostComments>> GetComments(int postId) // returns fitness post related comments
        {
            return await _dbContext.CommunityPostComments // table of comments in the database
                .Where(c => c.CommunityPostId == postId) // gets comments that are linked to a specific psot
                .ToListAsync(); // gets list
        }

        public async Task<bool> DeleteComment(int commentId)
        { 
            var comment = await _dbContext.CommunityPostComments.FindAsync(commentId); // finds the commentid in the database

            if (comment == null) // checks if the comment exists
            {
                return false; // return false if the comment does ot exist
            }

            var post = await _dbContext.CommunityPosts.FindAsync(comment.CommunityPostId); // finds the related fitnesspost

            if (post != null && post.CommentCount > 0) // return false if the comment does ot exist
            {
                post.CommentCount -= 1; // if a comment is deleted the comment counter would decrease
            }

            _dbContext.CommunityPostComments.Remove(comment); // deletes the comment in the database from the id input

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CommunityPostComments> UpdateComments(CommunityPostComments updatedComment) // updates the comment
        {
            var comment = await _dbContext.CommunityPostComments.FindAsync(updatedComment.CommunityPostCommentId); // searches the fitnesspostcommentid

            if (comment == null) // checks to see if comment exists and returns null if it doesnt
            {
                return null;
            }

            comment.Comment = updatedComment.Comment; // updates user data with the input

            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeletePost(int postId, int userId) // deletes post with user who created it 
        {
            var post = await _dbContext.CommunityPosts.FindAsync(postId); // finds the community post

            if (post == null || post.UserId != userId) // post needs to exist and only to be deleted by user who created it
            {
                return false;
            }

            _dbContext.CommunityPosts.Remove(post); // removes post from db
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> LikePost(int postId, int userId)  // adds a like to a  fitnesspost
        {
            var post = await _dbContext.CommunityPosts.FindAsync(postId);
            if (post == null) return false;

            var existingLike = await _dbContext.CommunityPostLikes
                .FirstOrDefaultAsync(l => l.CommunityPostId == postId && l.UserId == userId);

            if (existingLike != null)
            {
                _dbContext.CommunityPostLikes.Remove(existingLike);
                post.LikeCount = Math.Max(0, post.LikeCount - 1);
            }
            else
            {
                _dbContext.CommunityPostLikes.Add(new CommunityPostLike
                {
                    CommunityPostId = postId,
                    UserId = userId
                });
                post.LikeCount += 1;
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CommunityPost>> GetPostsByUserId(int userId)
        {
            return await _dbContext.CommunityPosts
                .Where(p => p.UserId == userId)
                .Include(p => p.Comments)
                .ToListAsync();
        }
    }
}
