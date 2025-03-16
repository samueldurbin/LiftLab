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

    }
}
