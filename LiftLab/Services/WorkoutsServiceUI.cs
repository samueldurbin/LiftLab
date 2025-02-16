using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.Services
{
    public class WorkoutsServiceUI
    {
        private readonly HttpClient _httpClient;

        public WorkoutsServiceUI()
        {
            _httpClient = new HttpClient  // Creates an instance of HTTP Client
            {
                BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP3000/SDurbin/api/")  // URL for api requests

            };
        }

        public async Task<List<Workouts>> GetAllWorkoutsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("WorkoutPlans/workouts"); // Replace with your actual endpoint

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Workouts>>();
                }

                throw new Exception($"Error fetching workouts: {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                // Log or rethrow exceptions as needed
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        // Create a new workout plan in the backend
        public async Task<bool> CreateWorkoutPlanAsync(string planName, List<int> workoutIds)
        {
            var request = new
            {
                PlanName = planName,
                WorkoutIds = workoutIds
            };

            var response = await _httpClient.PostAsJsonAsync("WorkoutPlans/createplan", request);

            return response.IsSuccessStatusCode;
        }
    }
}
