using LiftLab.ViewModels;
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

        public async Task<List<WorkoutInPlanDTO>> GetWorkoutDetailsForPlan(int planId)
        {
            var workouts = await _httpClient.GetAsync($"WorkoutPlans/getplanworkouts/{planId}");

            if (workouts.IsSuccessStatusCode)
            {
                return await workouts.Content.ReadFromJsonAsync<List<WorkoutInPlanDTO>>();
            }

            throw new Exception("Failed to get workout plan details.");
        }

        public async Task UpdateWorkoutInPlan(int planId, WorkoutInPlanDisplay workout)
        {
            var plan = new
            {
                WorkoutPlanId = planId,
                WorkoutId = workout.WorkoutId, // You need to add WorkoutId to WorkoutInPlanDisplay
                Reps = workout.Reps,
                Sets = workout.Sets,
                Kg = workout.Kg
            };

            var workouts = await _httpClient.PutAsJsonAsync("WorkoutPlans/updateworkoutinplan", plan);

            if (!workouts.IsSuccessStatusCode)
            {
                throw new Exception("Failed to update workout in plan.");
            }
        }
        public async Task<List<WorkoutPlans>> GetPlansByUserId(int userId) // gets a list of workout plans by userid
        {
            var plans = await _httpClient.GetAsync($"WorkoutPlans/getworkoutplansbyuser/{userId}"); // HTTP Get request for workout plans created by a userid

            if (plans.IsSuccessStatusCode) // checks if the response was succesful and will return a 200 code if success
            {
                return await plans.Content.ReadFromJsonAsync<List<WorkoutPlans>>(); // returns list of workout plans and deserialises it into a list of workout plans
            }

            throw new Exception("Failed to get workout plans for the logged in user"); // if the response variable was not successful it will throw this error
        }

        public async Task<List<int>> GetWorkoutsByPlanId(int planId) // this function will get a list of all the workout ids by workoutplans, which will be used for showing workoutnames
        {
            var workouts = await _httpClient.GetAsync($"WorkoutPlans/getplanworkouts/{planId}"); // HTTP Get Request for getting all the workoutids for a planid

            if (workouts.IsSuccessStatusCode) // checks if the response was succesful and will return a 200 code if success
            {
                return await workouts.Content.ReadFromJsonAsync<List<int>>();// returns list of workout ids
            }

            throw new Exception("Failed to get a list of workoutids"); // error message if the response is not a success
        }

        public async Task<List<WorkoutInPlanDTO>> GetWorkoutsByPlanIdForPopup(int planId)
        {
            var workouts = await _httpClient.GetAsync($"WorkoutPlans/getplanworkouts/{planId}");

            if (workouts.IsSuccessStatusCode)
            {
                return await workouts.Content.ReadFromJsonAsync<List<WorkoutInPlanDTO>>();
            }

            throw new Exception("Failed to get workout plan details.");
        }

        public async Task<WorkoutPlans> CreateWorkoutPlan(CreateWorkoutPlan workoutPlan) // method that creates a workout plan
        {
            var workouts = await _httpClient.PostAsJsonAsync("WorkoutPlans/createplan", workoutPlan); // HTTP post to create a workout plan

            if (workouts.IsSuccessStatusCode) // checks if the response was succesful and will return a 200 code if success
            {
                return await workouts.Content.ReadFromJsonAsync<WorkoutPlans>(); // shows the created workout plan
            }

            throw new Exception("Failed to create a new workout plan"); // error message if the response is not a success
        }

        public async Task<List<Workouts>> GetAllWorkouts()  // method to get a list of workouts
        {
            var workouts = await _httpClient.GetAsync("Workouts/getallworkouts"); // api endpoint to http get a list of workouts

            if (workouts.IsSuccessStatusCode) // checks if the response was succesful and will return a 200 code if success
            {
                return await workouts.Content.ReadFromJsonAsync<List<Workouts>>(); // deserialises and returns a list of workouts
            }

            throw new Exception("Failed to get workouts"); // error message 
        }

        public async Task<bool> DeleteWorkoutFromPlan(int workoutPlanId, int workoutId)
        {
            var workout = await _httpClient.DeleteAsync($"WorkoutPlans/removeworkoutfromplan/{workoutPlanId}/{workoutId}");

            return workout.IsSuccessStatusCode; // Returns true if the deletion was successful
        }

        public async Task<bool> DeleteWorkoutPlan(int planId)
        {
            var workouts = await _httpClient.DeleteAsync($"WorkoutPlans/deleteplan/{planId}");

            return workouts.IsSuccessStatusCode;
        }

    }
}
