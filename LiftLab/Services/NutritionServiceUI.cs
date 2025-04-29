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

        public async Task<bool> AddExternalUserMealPlan(int mealPlanId, int userId)
        {
            var response = await _httpClient.PostAsync($"MealPlanMeals/addusermealplan/{mealPlanId}/{userId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddExternalUserMeal(int mealId, int userId)
        {
            var response = await _httpClient.PostAsync($"MealPlanMeals/addusermeal/{mealId}/{userId}", null);
            return response.IsSuccessStatusCode;
        }


        public async Task<List<MealPlans>> GetMealPlansByMealId(int mealId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"MealPlanMeals/mealplansbymeal/{mealId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<MealPlans>>();
                }
                else
                {
                    throw new Exception("Failed to fetch meal plans for the meal.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching meal plans: {ex.Message}");
            }
        }

        public async Task<Meals> CreateMeal(Meals meal)
        {
            var response = await _httpClient.PostAsJsonAsync("MealPlanMeals/createmeal", meal);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Meals>();
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<MealPlans> CreateMealPlan(CreateMealPlanDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("MealPlanMeals/createmealplan", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MealPlans>();
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<List<Meals>> GetMealsByUser(int userId)
        {
            var response = await _httpClient.GetAsync($"MealPlanMeals/meals/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Meals>>();
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<List<MealPlans>> GetMealPlansByUser(int userId)
        {
            var response = await _httpClient.GetAsync($"MealPlanMeals/mealplans/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<MealPlans>>();
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> DeleteMealPlan(int mealPlanId)
        {
            var response = await _httpClient.DeleteAsync($"MealPlanMeals/deletemealplan/{mealPlanId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMeal(int mealId)
        {
            var response = await _httpClient.DeleteAsync($"MealPlanMeals/deletemeal/{mealId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Meals>> GetMealsByPlanId(int mealPlanId)
        {
            var response = await _httpClient.GetAsync($"MealPlanMeals/mealsbyplan/{mealPlanId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Meals>>();
            }

            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }

}

