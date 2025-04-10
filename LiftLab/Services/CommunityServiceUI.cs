using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.Services
{
    public class CommunityServiceUI
    {
        private readonly HttpClient _httpClient;
        public CommunityServiceUI()
        {
            _httpClient = new HttpClient // Creates an instance of HTTP Client
            {
                BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP3000/SDurbin/api/") // URL for api requests
            };
        }

        // Create Community Post
        public async Task<CommunityPost> CreatePost(int userId, string username, string imageUrl, string caption, int? workoutPlanId)
        {
            var response = await _httpClient.PostAsJsonAsync("CommunityPosts/createcommunitypost", new CommunityPost // sends a http get request to the fitnessposts endpoint
            {
                UserId = userId,
                Username = username,
                ImageUrl = imageUrl,
                Caption = caption,
                WorkoutPlanId = workoutPlanId
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CommunityPost>();
            }

            return null;
        }

        // Get All Community Posts
        public async Task<List<CommunityPost>> GetAllCommunityPosts()
        {
            var response = await _httpClient.GetAsync("CommunityPosts/getallcommunityposts"); // sends a http get request to the fitnessposts endpoint

            if (response.IsSuccessStatusCode) // checks to see whether it returns a successful status code (200)
            {
                return await response.Content.ReadFromJsonAsync<List<CommunityPost>>(); // returns the json body and deserializes the json into a fitnesspost object
            }

            throw new Exception("Failed to get community posts, please try again."); // failed to retrieve the posts exception message
        }

        // Get All Workout Plans
        public async Task<List<WorkoutPlans>> GetAllPlans() // gets all workoutplans as a list
        {
            var response = await _httpClient.GetAsync("WorkoutPlans/getallplans"); // sends a get request to the api, to retrieve the list of plans

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<WorkoutPlans>>(); // shows a 200 code if success
            }

            throw new Exception("Failed to get workout plans, please try again.");
        }

        // Add Comment to a Community Post
        public async Task<bool> CreateComment(int postId, string username, string comment)
        {
            var commentDto = new AddNewCommentDTO // a dto to match the api request
            {
                CommunityPostId = postId, // the fitnesspost that the comment is being added on
                Username = username, // the user who is adding the comment
                Comment = comment // the comment
            };

            var response = await _httpClient.PostAsJsonAsync("CommunityPosts/addcomment", commentDto); // http post request to add comment to fitnesspost

            return response.IsSuccessStatusCode;
        }

        // Get Comments for a Post
        public async Task<List<CommunityPostComments>> GetCommentsByPost(int postId)
        {
            var response = await _httpClient.GetAsync($"CommunityPosts/comments/{postId}"); // recieves comments from a specific post

            if (response.IsSuccessStatusCode) // this helps postman request testing
            {
                return await response.Content.ReadFromJsonAsync<List<CommunityPostComments>>(); 
            }

            return new List<CommunityPostComments>(); // returns a lsit of fitnesspost comments
        }

        // Like a Post
        public async Task<bool> LikePost(int postId, int userId)
        {
            var response = await _httpClient.PostAsync($"CommunityPosts/like/{postId}/{userId}", null); // sends a http post request and not sending a body (only uses parameters)
            return response.IsSuccessStatusCode;
        }

        // Add External Plan to User
        public async Task<bool> AddExternalUserWorkoutPlan(int planId, int userId)
        {
            var response = await _httpClient.PostAsync($"WorkoutPlans/adduserworkoutplan/{planId}/{userId}", null); // sends a http post request with the ids
            return response.IsSuccessStatusCode;  // returns true if success
        }

        // Gets userid for profile viewing
        public async Task<Users> GetUserById(int userId)
        {
            var response = await _httpClient.GetAsync($"Users/getuserbyid/{userId}"); // aoi endpoint with the inserted userId

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Users>();
            }

            return null;
        }
    }

}

