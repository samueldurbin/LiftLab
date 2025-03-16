using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi.Services
{
    public class FriendService : IFriendService
    {
        private readonly AppDbContext _dbContext; // to use the database

        public FriendService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // friends are created by a userid and friendUserId (different userid in database) are added together
        public async Task<bool> AddFriend(int userId, int friendUserId) // function to add friends 
        {
            if (userId == friendUserId)
            {
                return false; // this stops users adding themselves as a friend
            } 

            var existingFriends = await _dbContext.Friends // friends table in the database
                .AnyAsync(u => (u.UserId == userId && u.FriendUserId == friendUserId) ||
                               (u.UserId == friendUserId && u.FriendUserId == userId)); // this checks the database to see if a friendship between the two userids exist in
                                                                                        // in the database, and the OR allows the check for other way around

            if (existingFriends) 
            {
                return false; // this prevents two friends from being added whilst they are already added

            };

            var newFriend = new Friend // creates friends in the database
            {
                UserId = userId, // user who is adding a friend
                FriendUserId = friendUserId // friend being added
            };

            _dbContext.Friends.Add(newFriend); // add to the Friends database
            await _dbContext.SaveChangesAsync(); // save changes
            return true; // success
        }

        public async Task<List<int>> GetUsersFriends(int userId) // list of friends associated with a user
        {
            return await _dbContext.Friends // friends table in the database
                .Where(u => u.UserId == userId) // in the table where the input user is the one adding the new friends
                .Select(u => u.FriendUserId) // this is selecting only the ids of the users friends
                .ToListAsync(); // to a list
        }

        public async Task<List<FitnessPost>> GetFriendsPosts(int userId) // list of all the fitnessposts from who the user has added as a friend
        {
            var friendsIds = await GetUsersFriends(userId); // gets all of the friend ids

            return await _dbContext.FitnessPosts // database table of fitnessposts
                .Where(p => _dbContext.Users // usernames and userids are stored
                    .Where(u => friendsIds.Contains(u.UserId)) // all users in the friends list
                    .Select(u => u.Username) // the usernames of the ids
                    .Contains(p.Username)) // fitnessposts username that matches a certain username
                .ToListAsync(); // to a list
        }

    }
}
