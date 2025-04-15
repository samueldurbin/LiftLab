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

        #region CreatePost

        // this method creates a community post (either fitness or nutritional) using the user input and sends to the database through the below api post request
        // userid is the user creating the post which has preferences in the frontend to associate each created post with the logged in user
        public async Task<CommunityPost> CreatePost(int userId, string username, string caption, int? workoutPlanId, int? mealId, int? mealPlanId) // workoutplanid, mealplanid, and mealid are all nullable objects so users can select what they want
        {
            // sends a http get request to the communityposts create endpoint
            var communityPost = await _httpClient.PostAsJsonAsync("CommunityPosts/createcommunitypost", new CommunityPost // creates a new CommunityPost object to send to backend
            {
                UserId = userId, // who is creating the post
                Username = username, // the username of the id creating the post, this is mainly for ui display
                Caption = caption, // the caption of the post
                WorkoutPlanId = workoutPlanId, // the id of a workoutplan which is optional to post
                MealId = mealId, // the id of a meal which is optional to post
                MealPlanId = mealPlanId, // the id of a mealplan which is optional to post
                CreatedDate = DateTime.UtcNow // datetime for when post was created which helps with showing most recent post
            });

            if (communityPost.IsSuccessStatusCode) // this checks if the http post request was successful
            {
                return await communityPost.Content.ReadFromJsonAsync<CommunityPost>(); // returns communitypost object if successful
            }

            return null; // this would return null is the createpost request was not successful
        }

        #endregion


        #region Get Posts & Plans

        // this method gets all of the communityposts that are stored in the database and is called through a http get request using the below api from the controller
        // this method will return a list of communityposts
        public async Task<List<CommunityPost>> GetAllCommunityPosts()
        {
            var communityPosts = await _httpClient.GetAsync("CommunityPosts/getallcommunityposts"); // sends a http get request to the communityposts endpoint

            if (communityPosts.IsSuccessStatusCode) // checks to see whether it returns a successful status code (200)
            {
                return await communityPosts.Content.ReadFromJsonAsync<List<CommunityPost>>(); // returns the json body and deserializes the json into a communitypost object
            }

            throw new Exception("Failed to get community posts, please try again!"); // failed to retrieve the posts exception message
        }

        // this method gets the workoutplans that were created/added by a user (not requesting every workoutplan)
        // uses the userId to retrieve a list of workoutplans
        public async Task<List<WorkoutPlans>> GetWorkoutPlansByUserId(int userId)
        {
            var workoutPlans = await _httpClient.GetAsync($"WorkoutPlans/getworkoutplansbyuser/{userId}"); // sends a http get request to the api, to retrieve the list of plans

            if (workoutPlans.IsSuccessStatusCode) // checks for success
            {
                return await workoutPlans.Content.ReadFromJsonAsync<List<WorkoutPlans>>(); // shows a 200 code if success
            }

            throw new Exception("Failed to get workout plans, please try again!");
        }

        // this method gets all the mealplans created/added by a userid and retrieves them as a list
        public async Task<List<MealPlans>> GetMealPlansByUserId(int userId)
        {
            var mealPlans = await _httpClient.GetAsync($"MealPlans/getmealplansbyuser/{userId}"); // sends a http get request to the api, to retrieve the list of mealplans

            if (mealPlans.IsSuccessStatusCode)
            {
                return await mealPlans.Content.ReadFromJsonAsync<List<MealPlans>>(); // shows a 200 code if success
            }

            throw new Exception("Failed to get workout plans, please try again!"); // exception message
        }

        // this method gets all the individual meals from the database
        // returns them as a list
        public async Task<List<Meals>> GetMealsByUserId(int userId)
        {
            var meals = await _httpClient.GetAsync($"MealPlans/meals/{userId}"); // sends a http get request with the input userId

            if (meals.IsSuccessStatusCode)
            {
                return await meals.Content.ReadFromJsonAsync<List<Meals>>();
            }

            throw new Exception("Failed to get meals, please try again.");
        }

        #endregion


        #region Comments
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

        public async Task<List<CommunityPostComments>> GetCommentsByPost(int postId)
        {
            var response = await _httpClient.GetAsync($"CommunityPosts/comments/{postId}"); // recieves comments from a specific post

            if (response.IsSuccessStatusCode) // this helps postman request testing
            {
                return await response.Content.ReadFromJsonAsync<List<CommunityPostComments>>(); 
            }

            return new List<CommunityPostComments>(); // returns a lsit of fitnesspost comments
        }

        #endregion


        #region Post Liking
        public async Task<bool> LikePost(int postId, int userId)
        {
            var response = await _httpClient.PostAsync($"CommunityPosts/like/{postId}/{userId}", null); // sends a http post request and not sending a body (only uses parameters)
            return response.IsSuccessStatusCode;
        }
        #endregion

        public async Task<List<Meals>> GetMealsByUser(int userId)
        {
            var response = await _httpClient.GetAsync($"MealPlans/meals/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Meals>>();
            }
            throw new Exception("Failed to get meals for the user");
        }

        #region Adding External Plans and Meals

        public async Task<bool> AddExternalUserWorkoutPlan(int planId, int userId)
        {
            var externalWorkoutPlan = await _httpClient.PostAsync($"WorkoutPlans/adduserworkoutplan/{planId}/{userId}", null); // sends a http post request with the ids

            if (externalWorkoutPlan.IsSuccessStatusCode)
            {
                return true; // success
            }

            throw new Exception("Failed to add workout plan to user, please try again!"); // exception message
        }

        public async Task<bool> AddExternalUserMealPlans(int mealPlanId, int userId)
        {
            var externalMealPlan = await _httpClient.PostAsync($"MealPlans/addusermealplan/{mealPlanId}/{userId}", null); // sends a http post request with the ids

            if (externalMealPlan.IsSuccessStatusCode)
            {
                return true; // success
            }

            throw new Exception("Failed to add workout plan to user, please try again!"); // exception message
        }

        public async Task<bool> AddExternalUserMeals(int mealId, int userId)
        {
            var externalMeals = await _httpClient.PostAsync($"MealPlans/addusermeal/{mealId}/{userId}", null); // sends a http post request with the ids

            if (externalMeals.IsSuccessStatusCode)
            {
                return true; // success
            }

            throw new Exception("Failed to add workout plan to user, please try again!"); // exception message
        }


        public async Task<bool> AddExternalPlans(CommunityPost post, int userId) // instance of a communitypost with a userid
        {
            if (post.WorkoutPlanId != null) // checks if a community post has a workoutplan attached
            {
                return await AddExternalUserWorkoutPlan(post.WorkoutPlanId.Value, userId); // if it does, call this method
            }
            else if (post.MealPlanId != null) // checks if a community post has a mealplan attached
            {
                return await AddExternalUserMealPlans(post.MealPlanId.Value, userId);
            }
            else if (post.MealId != null) // checks if a community post has a meal attached
            {
                return await AddExternalUserMeals(post.MealId.Value, userId);
            }

            return false;
        }

        #endregion


        #region Profile

        public async Task<Users> GetUserById(int userId)
        {
            var response = await _httpClient.GetAsync($"Users/getuserbyid/{userId}"); // aoi endpoint with the inserted userId

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Users>();
            }

            return null;
        }

        #endregion

        public async Task<List<CommunityPost>> GetCommunityPostsByUserId(int userId)
        {
            var response = await _httpClient.GetAsync($"CommunityPosts/getpostsbyuser/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CommunityPost>>();
            }

            return new List<CommunityPost>();
        }

        public async Task<bool> DeleteCommunityPost(int postId, int userId)
        {
            var response = await _httpClient.DeleteAsync($"CommunityPosts/deletepost/{postId}/{userId}"); //http delete request
            return response.IsSuccessStatusCode;
        }
    }

}

