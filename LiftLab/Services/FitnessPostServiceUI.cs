using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Shared.Models;

namespace LiftLab.Services
{
    public class FitnessPostServiceUI
    {
        private readonly HttpClient _httpClient;

        public FitnessPostServiceUI()
        {
            _httpClient = new HttpClient  // Creates an instance of HTTP Client
            {
                BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP3000/SDurbin/api/")  // URL for api requests

            };
        }

        public async Task<FitnessPost> CreatePost(string username, string imageUrl, string caption, DateTime createdDate)  // Entered details
        {
            var response = await _httpClient.PostAsJsonAsync("Fitnesspost/createpost", new FitnessPost // Create an Account EndPoint
            {
                Username = username,
                ImageUrl = imageUrl,
                Caption = caption,
                CreatedDate = createdDate,
             
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<FitnessPost>();
            }

            return null;
        }

        public async Task<List<FitnessPost>> GetAllFitnessPosts()
        {
            var response = await _httpClient.GetAsync("Fitnesspost/fitnessposts"); // sends a http get request to the fitnessposts endpoint

            if (response.IsSuccessStatusCode) // checks to see whether it returns a successful status code (200)
            {
                return await response.Content.ReadFromJsonAsync<List<FitnessPost>>(); // returns the json body and deserializes the json into a fitnesspost object
            }

            throw new Exception("Failed to get fitnessposts, please try again"); // failed to retrieve the posts exception message
        }

    }
}
