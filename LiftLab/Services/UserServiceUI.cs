using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.Services
{
    public class UserServiceUI
    {
        private readonly HttpClient _httpClient;

        public UserServiceUI()
        {
            _httpClient = new HttpClient  // Creates an instance of HTTP Client
            {
                BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP3000/SDurbin/api/")  // URL for api requests

            };
        }

        public async Task<Users> Login(string username, string password) // Asynchronous Task for Login
        {
            var login = await _httpClient.PostAsJsonAsync("Users/login", new Users // Login an Account EndPoint
            {
                Username = username,  // Checks the username matches the entered username
                Password = password

            });

            if (login.IsSuccessStatusCode) // Checks for success from call
            {
                return await login.Content.ReadFromJsonAsync<Users>();  // Deserializes the JSON request body into the Model object

            }

            return null;
        }

        public async Task<Users> CreateAccount(string username, string password, string email, string mobileNumber, DateTime dateOfBirth)  // Entered details
        {
            var response = await _httpClient.PostAsJsonAsync("Users/register", new Users // Create an Account EndPoint
            {
                Username = username, // matches with user inputs to save to database through api
                Password = password,
                Email = email,
                MobileNumber = mobileNumber,
                DateOfBirth = dateOfBirth
            });

            if (response.IsSuccessStatusCode) // returns a success message in postman for testing
            {
                return await response.Content.ReadFromJsonAsync<Users>();
            }

            return null;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            var response = await _httpClient.GetAsync("Users/getallusers"); // get request to get a list of all users

            if (response.IsSuccessStatusCode) // responds with success message
            {
                return await response.Content.ReadFromJsonAsync<List<Users>>();
            }

            throw new Exception("Failed to get any users."); // error message
        }
    }
}
