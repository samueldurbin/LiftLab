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

        public async Task<Meals> CreateMeal(Meals meal)
        {
            var createMeal = await _httpClient.PostAsJsonAsync("MealPlans/createmeal", meal);

            if (createMeal.IsSuccessStatusCode)
            {
                return await createMeal.Content.ReadFromJsonAsync<Meals>();
            }

            throw new Exception(await createMeal.Content.ReadAsStringAsync());
        }

        public async Task<MealPlans> CreateMealPlan(CreateMealPlanDTO dto) // sends the dto to the api
        {
            var response = await _httpClient.PostAsJsonAsync("MealPlans/createmealplan", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MealPlans>();
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<List<Meals>> GetMealsByUser(int userId)
        {
            var getMealsByUser = await _httpClient.GetAsync($"MealPlans/meals/{userId}");

            if (getMealsByUser.IsSuccessStatusCode)
            {
                return await getMealsByUser.Content.ReadFromJsonAsync<List<Meals>>();
            }

            throw new Exception("Failed to get meals for the user");
        }

        public async Task<List<MealPlans>> GetMealPlansByUser(int userId)
        {
            var getPlansByUser = await _httpClient.GetAsync($"MealPlans/getmealplansbyuser/{userId}");

            if (getPlansByUser.IsSuccessStatusCode)
            {
                return await getPlansByUser.Content.ReadFromJsonAsync<List<MealPlans>>();
            }

            throw new Exception("Failed to get meal plans for the user");
        }
    }
}
