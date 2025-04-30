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

        #region CommunityPosts
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

        public async Task<IEnumerable<CommunityPost>> GetPostsByUserId(int userId)
        {
            return await _dbContext.CommunityPosts
                .Where(p => p.UserId == userId) // gets all the posts the userId matches
                .Include(p => p.Comments) // loads comments related to the postid
                .ToListAsync();
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

        #endregion

        #region Comments

        public async Task<CommunityPostComments> AddComment(CommunityPostComments comment) 
        {
            var post = await _dbContext.CommunityPosts.FindAsync(comment.CommunityPostId); // finds the post from the database using the postid

            if (post == null)
            {
                throw new Exception("Post has not been found, Please try again"); // this is mainly for backend testing as a post that did not exist would not show in the front end
            }

            _dbContext.CommunityPostComments.Add(comment); // adds the comment to the post

            post.CommentCount += 1; // this increments the comment counter in the database

            await _dbContext.SaveChangesAsync(); // saves both the comment and the counter
            return comment;
        }

        public async Task<IEnumerable<CommunityPostComments>> GetComments(int postId) // returns post related comments
        {
            return await _dbContext.CommunityPostComments // table of comments in the database
                .Where(c => c.CommunityPostId == postId) // gets comments that are linked to a specific post
                .ToListAsync(); // gets list
        }

        #endregion

    }
}
