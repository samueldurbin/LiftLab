using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Azure;

namespace LiftLab.Services
{
    public class UsersServiceUI
    {
        private readonly HttpClient _httpClient; // this sends requests to the backend (http requests)

        public UsersServiceUI()
        {
            _httpClient = new HttpClient  // creates an instance of httpclient
            {
                BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP3000/SDurbin/api/")  // URL for api requests (which requires additional endpoints for functions)

            };
        }

        public async Task<Users> Login(string username, string password) // login function
        {
            // sends a post request
            var login = await _httpClient.PostAsJsonAsync("Users/login", new LoginRequest // api endpoint for the login
            {
                Username = username, // this checks if the users input matches a record in the database
                Password = password

            });

            if (login.IsSuccessStatusCode) // checks for success
            {
                return await login.Content.ReadFromJsonAsync<Users>();  // Deserializes the JSON request body into the Model object

            }

            return null; // if failed
        }

        public async Task<Users> CreateAccount(string username, string password, string email, string mobileNumber, DateTime dateOfBirth)  // create an account function
        {
            var account = await _httpClient.PostAsJsonAsync("Users/createuser", new Users // create an account endpoint
            {
                Username = username, // saves the users input to the records required in the database
                Password = password,
                Email = email,
                MobileNumber = mobileNumber,
                DateOfBirth = dateOfBirth
            });

            if (account.IsSuccessStatusCode) // returns a success message in postman for testing
            {
                return await account.Content.ReadFromJsonAsync<Users>();
            }

            return null;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            var users = await _httpClient.GetAsync("Users/getallusers"); // get request to get a list of all users

            if (users.IsSuccessStatusCode) // responds with success message
            {
                return await users.Content.ReadFromJsonAsync<List<Users>>();
            }

            throw new Exception("Failed to get any users."); // error message
        }

        public async Task<Users> GetUserById(int id)
        {
            var user = await _httpClient.GetAsync($"Users/getuserbyid/{id}"); // gets user by input userid

            if (user.IsSuccessStatusCode)
            {
                return await user.Content.ReadFromJsonAsync<Users>();
            }

            return null;
        }

        public async Task<Users> UpdateUser(Users updatedUser)
        {
            var user = await _httpClient.PutAsJsonAsync($"Users/{updatedUser.UserId}", updatedUser); // updates the user based on the input id

            if (user.IsSuccessStatusCode)
            {
                return await user.Content.ReadFromJsonAsync<Users>();
            }

            return null;
        }
    }
}
