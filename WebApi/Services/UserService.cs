using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Utilities;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Users> LoginAuthentication(string username, string password)
        {
            string hashPassword = Hashing.Hash(password); // allows the hash value to be compared by input

            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashPassword); // matches the username and password to simulate authentication
        }

        public async Task<IEnumerable<Users>> GetUsers() // returns a colletion of fitness posts
        {
            return await _dbContext.Users
                                 .ToListAsync();
        }

        public async Task<Users> CreateUser(Users user) // adds post to the database
        {
            //var hash = new Hashing(); // hashing method from the utility class
            //user.Password = hash.Hash(user.Password); // hashes the password before sending to database

            user.Password = Hashing.Hash(user.Password);

            _dbContext.Users.Add(user); // adds the user to te table

            await _dbContext.SaveChangesAsync(); // saves

            return user;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId); // finds the userid

            if (user == null) // checks if the user exists
            {
                return false;
            }

            _dbContext.Users.Remove(user); // deletes the user in the database from the id input
            await _dbContext.SaveChangesAsync();

            return true;

        }

        public async Task<Users?> UpdateUser(Users updatedUser) // updates the user
        {
            var user = await _dbContext.Users.FindAsync(updatedUser.UserId); // searches the id

            if(user == null) // checks to see if user exists
            {
                return null;
            }
            
            user.Firstname = updatedUser.Firstname; // updates user data with the input

            user.Lastname = updatedUser.Lastname;
            
            user.Email = updatedUser.Email;
            
            user.MobileNumber = updatedUser.MobileNumber;
            
            user.Username = updatedUser.Username;

            user.DateOfBirth = updatedUser.DateOfBirth;
            
            user.AccountCreationDate = updatedUser.AccountCreationDate;
            
            //var hash = new Hashing();
            user.Password = Hashing.Hash(updatedUser.Password); // hashes the new updated password


            await _dbContext.SaveChangesAsync();

            return user;
            
        }
    }
}
