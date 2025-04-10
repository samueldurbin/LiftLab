using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.Services
{
    public class NutritionServiceUI
    {
        private readonly HttpClient _httpClient;
        public NutritionServiceUI()
        {
            _httpClient = new HttpClient // Creates an instance of HTTP Client
            {
                BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP3000/SDurbin/api/") // URL for api requests
            };
        }
        public async Task<List<Meal>> GetMealsByUserId(int userId)
        {
            var response = await _httpClient.GetAsync($"MealPlans/meals/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Meal>>();
            }
            throw new Exception("Failed to get meals for the user");
        }

        public async Task<List<MealPlans>> GetMealPlansByUser(int userId)
        {
            var response = await _httpClient.GetAsync($"MealPlans/getmealplansbyuser/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<MealPlans>>();
            }
            throw new Exception("Failed to get meal plans for the user");
        }
    }
}
