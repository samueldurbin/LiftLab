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

        public async Task<UserModel> LoginAsync(string username, string password) // Asynchronous Task for Login
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", new UserModel // Login an Account EndPoint
            {
                Username = username,  // Checks the username matches the entered username
                Password = password

            });

            if (response.IsSuccessStatusCode) // Checks for success from call
            {
                return await response.Content.ReadFromJsonAsync<UserModel>();  // Deserializes the JSON request body into the Model object

            }

            return null;
        }

        public async Task<UserModel> CreateAccountAsync(string username, string password, string email, string phoneNumber, DateTime dateOfBirth)  // Entered details
        {
            var response = await _httpClient.PostAsJsonAsync("auth/register", new UserModel // Create an Account EndPoint
            {
                Username = username,
                Password = password,
                Email = email,
                PhoneNumber = phoneNumber,
                DateOfBirth = dateOfBirth
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserModel>();
            }

            return null;
        }
    }
}
