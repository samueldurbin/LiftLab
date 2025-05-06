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
            var mealPlan = await _httpClient.PostAsync($"MealPlanMeals/addusermealplan/{mealPlanId}/{userId}", null); // post request to copy a meal plan to a specific user
            return mealPlan.IsSuccessStatusCode; // return true if sucessful
        }

        public async Task<bool> AddExternalUserMeal(int mealId, int userId)
        {
            var meal = await _httpClient.PostAsync($"MealPlanMeals/addusermeal/{mealId}/{userId}", null); // post request to copy a meal to a specific user

            return meal.IsSuccessStatusCode; // return true if successful
        }

        public async Task<Meals> CreateMeal(Meals meal)
        {
            var createdMeal = await _httpClient.PostAsJsonAsync("MealPlanMeals/createmeal", meal); // post request with meal data as JSON body

            if (createdMeal.IsSuccessStatusCode) // if it was successful, read and reutn the meal object
            {
                return await createdMeal.Content.ReadFromJsonAsync<Meals>();
            }

            throw new Exception(await createdMeal.Content.ReadAsStringAsync());
        }

        public async Task<MealPlans> CreateMealPlan(CreateMealPlanDTO mealPlan)
        {
            var createdMealPlan = await _httpClient.PostAsJsonAsync("MealPlanMeals/createmealplan", mealPlan); // post request to create a new meal plan with the related meal ids

            if (createdMealPlan.IsSuccessStatusCode) // checks for success
            {
                return await createdMealPlan.Content.ReadFromJsonAsync<MealPlans>();
            }

            throw new Exception(await createdMealPlan.Content.ReadAsStringAsync());
        }

        public async Task<List<Meals>> GetMealsByUser(int userId)
        {
            var meals = await _httpClient.GetAsync($"MealPlanMeals/meals/user/{userId}"); // get request with userid

            if (meals.IsSuccessStatusCode)
            {
                return await meals.Content.ReadFromJsonAsync<List<Meals>>();
            }

            throw new Exception(await meals.Content.ReadAsStringAsync());
        }

        public async Task<List<MealPlans>> GetMealPlansByUser(int userId)
        {
            var mealPlans = await _httpClient.GetAsync($"MealPlanMeals/mealplans/user/{userId}");  // get request

            if (mealPlans.IsSuccessStatusCode)
            {
                return await mealPlans.Content.ReadFromJsonAsync<List<MealPlans>>(); // list of mealplans
            }

            throw new Exception(await mealPlans.Content.ReadAsStringAsync());
        }

        public async Task<bool> DeleteMealPlan(int mealPlanId)
        {
            var deleteMealPlan = await _httpClient.DeleteAsync($"MealPlanMeals/deletemealplan/{mealPlanId}");

            return deleteMealPlan.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMeal(int mealId)
        {
            var deleteMeal = await _httpClient.DeleteAsync($"MealPlanMeals/deletemeal/{mealId}");

            return deleteMeal.IsSuccessStatusCode;
        }

        public async Task<List<Meals>> GetMealsByPlanId(int mealPlanId) // gets meals that are in a plan
        {
            var meals = await _httpClient.GetAsync($"MealPlanMeals/mealsbyplan/{mealPlanId}");

            if (meals.IsSuccessStatusCode)
            {
                return await meals.Content.ReadFromJsonAsync<List<Meals>>();
            }

            throw new Exception(await meals.Content.ReadAsStringAsync());
        }
    }

}

