using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.Services
{
    public class NutritionPostServiceUI
    {
        private readonly HttpClient _httpClient;

        public NutritionPostServiceUI()
        {
            _httpClient = new HttpClient  // Creates an instance of HTTP Client
            {
                BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP3000/SDurbin/api/")  // URL for api requests

            };
        }

        public async Task<NutritionPost> CreatePost(string username, string imageUrl, string caption)  // Entered details // int? due to being nullable
        {
            var response = await _httpClient.PostAsJsonAsync("Nutritionposts/createnutritionpost", new NutritionPost // Create an Account EndPoint
            {
                Username = username, // posts the entries 
                ImageUrl = imageUrl,
                Caption = caption

            });

            if (response.IsSuccessStatusCode) // returns 200 code if correct, helps with postman
            {
                return await response.Content.ReadFromJsonAsync<NutritionPost>();
            }

            return null;
        }

        public async Task<List<NutritionPost>> GetAllNutritionPosts()
        {
            var response = await _httpClient.GetAsync("Nutritionposts/getallnutritionposts"); // sends a http get request to the fitnessposts endpoint

            if (response.IsSuccessStatusCode) // checks to see whether it returns a successful status code (200)
            {
                return await response.Content.ReadFromJsonAsync<List<NutritionPost>>(); // returns the json body and deserializes the json into a fitnesspost object
            }

            throw new Exception("Failed to get fitnessposts, please try again"); // failed to retrieve the posts exception message
        }


    }
}

