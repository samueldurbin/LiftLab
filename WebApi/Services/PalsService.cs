
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApi.Services
{
    public class PalsService : IPalsService
    {
        private readonly AppDbContext _dbContext;

        public PalsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetMyPals(int userId) // userid from database comes into method to see the number of pals
        {
            var user = await _dbContext.Users // checks the user table not the pals as thats where the count is being stored
                .Where(u=>u.UserId == userId) // finds the correct user's against the input userid
                .Select(u=>u.MyPals) // selects from the MyPals column(which is the count)
                .FirstOrDefaultAsync(); // returns the matching request or null if nothing is found
            return user; // returns the response
        }

        public async Task<Pals> AddPals (Pals addPals) // method to add pals
        {
            _dbContext.Pals.Add(addPals); // adds entry
            await _dbContext.SaveChangesAsync(); // saves changes to the database
            return addPals; // returns the new Pals

        }
    }
}
