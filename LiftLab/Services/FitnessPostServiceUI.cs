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

        public async Task<FitnessPost> CreatePost(int userId, string username, string imageUrl, string caption, int? workoutPlanId)
        {
            var response = await _httpClient.PostAsJsonAsync("FitnessPost/createfitnesspost", new FitnessPost
            {
                UserId = userId,
                Username = username,
                ImageUrl = imageUrl,
                Caption = caption,
                WorkoutPlanId = workoutPlanId
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<FitnessPost>();
            }

            return null;
        }

        public async Task<List<FitnessPost>> GetAllFitnessPosts()
        {
            var response = await _httpClient.GetAsync("FitnessPost/getallfitnessposts"); // sends a http get request to the fitnessposts endpoint

            if (response.IsSuccessStatusCode) // checks to see whether it returns a successful status code (200)
            {
                return await response.Content.ReadFromJsonAsync<List<FitnessPost>>(); // returns the json body and deserializes the json into a fitnesspost object
            }

            throw new Exception("Failed to get fitnessposts, please try again"); // failed to retrieve the posts exception message
        }

        public async Task<List<WorkoutPlans>> GetAllPlans() // gets all workoutplans as a list
        {
            var response = await _httpClient.GetAsync("WorkoutPlans/getallplans"); // sends a get request to the api, to retrieve the list of plans
            // stores the response
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<WorkoutPlans>>(); // shows a 200 code if success
            }

            throw new Exception("Failed to get workout plans, please try again.");
        }

        public async Task<FitnessPostComments> AddComment(string username, string comment, int fitnessPostId)
        {
            var response = await _httpClient.PostAsJsonAsync("FitnessPost/addcomment", new FitnessPostComments // http post request to add comment to fitnesspost
            {
                Username = username, // hardcoded in api currently
                Comment = comment, // adds the input to the new comment
                FitnessPostId = fitnessPostId

            });

            if (response.IsSuccessStatusCode)  // checks for successs
            {
                return await response.Content.ReadFromJsonAsync<FitnessPostComments>(); // returns new comment
            }

            return null; // return null if not added correctly
        }

        public async Task<List<FitnessPostComments>> GetCommentsByPost(int postId)
        {
            var response = await _httpClient.GetAsync($"Fitnesspost/comments/{postId}"); // recieves comments from a specific post

            if (response.IsSuccessStatusCode) // this helps postman request testing
            {
                return await response.Content.ReadFromJsonAsync<List<FitnessPostComments>>();
            }

            return new List<FitnessPostComments>(); // returns a lsit of fitnesspost comments
        }



    }

    
}
