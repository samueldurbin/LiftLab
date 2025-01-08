using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.Services
{
    public class ApiUserService
    {
        private readonly HttpClient _httpClient;

        public ApiUserService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://cent-5-534.uopnet.plymouth.ac.uk/COMP3000/SDurbin/api/")
            };
        }

        public async Task<UserModel> LoginAsync(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", new UserModel
            {
                Username = username,
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserModel>();
            }

            return null;
        }
    }
}
