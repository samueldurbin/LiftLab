using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.Services
{
    public class WorkoutPlansServiceUI
    {
        private readonly HttpClient _httpClient;

        public WorkoutPlansServiceUI()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP3000/SDurbin/api/")

            };
        }

        public async Task<WorkoutPlans> CreatePlan(CreateWorkoutPlan request)
        {
            var response = await _httpClient.PostAsJsonAsync("WorkoutPlans/createplan", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<WorkoutPlans>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create plan with workouts: {errorMessage}");
            }
        }

        public async Task<List<Workouts>> GetAllWorkouts()
        {
            var response = await _httpClient.GetAsync("Workouts/getallworkouts");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Workouts>>();
            }

            throw new Exception("Failed to get workouts, please try again");
        }

    }
}
