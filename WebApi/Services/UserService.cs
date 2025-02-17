using Shared.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Utilities;
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
            var hash = new Hashing();
            string hashPassword = hash.Hash(password);

            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashPassword);
        }

        public async Task<IEnumerable<Users>> GetUsers() // returns a colletion of fitness posts
        {
            return await _dbContext.Users
                                 .ToListAsync();
        }

        public async Task<Users> CreateUser(Users user) // adds post to the database
        {
            var hash = new Hashing();
            user.Password = hash.Hash(user.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return false;
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return true;

        }

        public async Task<Users?> UpdateUser(Users updatedUser)
        {
            var user = await _dbContext.Users.FindAsync(updatedUser.UserId);

            if(user == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.Firstname))
                user.Firstname = updatedUser.Firstname;

            if (!string.IsNullOrWhiteSpace(updatedUser.Lastname))
                user.Lastname = updatedUser.Lastname;

            if (!string.IsNullOrWhiteSpace(updatedUser.Email))
                user.Email = updatedUser.Email;

            if (!string.IsNullOrWhiteSpace(updatedUser.MobileNumber))
                user.MobileNumber = updatedUser.MobileNumber;

            if (!string.IsNullOrWhiteSpace(updatedUser.Username))
                user.Username = updatedUser.Username;

            if (updatedUser.DateOfBirth != default)
                user.DateOfBirth = updatedUser.DateOfBirth;

            if (updatedUser.AccountCreationDate != default)
                user.AccountCreationDate = updatedUser.AccountCreationDate;

            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                var hash = new Hashing();
                user.Password = hash.Hash(updatedUser.Password);
            }

            await _dbContext.SaveChangesAsync();

            return user;
            
        }
    }
}
