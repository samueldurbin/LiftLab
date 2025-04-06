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

        // Post

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

        // Plans
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

        //// Comments
        //public async Task<bool> CreateComment(int postId, string username, string comment) 
        //{
        //    var commentDto = new AddNewCommentDTOF // a dto to match the api request
        //    {
        //        FitnessPostId = postId, // the fitnesspost that the comment is being added on
        //        Username = username, // the user who is adding the comment
        //        Comment = comment // the comment
        //    };

        //    var response = await _httpClient.PostAsJsonAsync("FitnessPost/addcomment", commentDto); // http post request to add comment to fitnesspost

        //    return response.IsSuccessStatusCode;
        //}

        public async Task<List<FitnessPostComments>> GetCommentsByPost(int postId)
        {
            var response = await _httpClient.GetAsync($"Fitnesspost/comments/{postId}"); // recieves comments from a specific post

            if (response.IsSuccessStatusCode) // this helps postman request testing
            {
                return await response.Content.ReadFromJsonAsync<List<FitnessPostComments>>();
            }

            return new List<FitnessPostComments>(); // returns a lsit of fitnesspost comments
        }

        // Likes
        public async Task<bool> LikePost(int postId, int userId) // method to like post
        {
            var response = await _httpClient.PostAsync($"FitnessPost/like/{postId}/{userId}", null); // sends a http post request and not sending a body (only uses parameters)

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddExternalUserWorkoutPlan(int planId, int userId)
        {
            var response = await _httpClient.PostAsync($"WorkoutPlans/adduserworkoutplan/{planId}/{userId}", null); // sends a http post request with the ids

            return response.IsSuccessStatusCode; // returns true if success
        }
    }
    
}
