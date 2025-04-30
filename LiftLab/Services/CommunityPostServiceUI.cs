using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.Services
{
    public class CommunityPostServiceUI
    {
        // this will be used to sned Http requests and recieve respomses from the uri
        private readonly HttpClient _httpClient; // readonly instance of HttpClient

        public CommunityPostServiceUI()
        {
            _httpClient = new HttpClient // creates an instance of HTTP Client
            {
                BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP3000/SDurbin/api/") // URL for api requests
            };
        }

        #region Create Post

        // this method creates a community post (either fitness or nutritional) using the user input and sends to the database through the below api post request
        // userid is the user creating the post which has preferences in the frontend to associate each created post with the logged in user
        public async Task<CommunityPost> CreatePost(int userId, string username, string caption, int? workoutPlanId, int? mealId, int? mealPlanId) // workoutplanid, mealplanid, and mealid are all nullable objects so users can select what they want
        {

            var communityPost = await _httpClient.PostAsJsonAsync("CommunityPosts/createpost", new CommunityPost // creates a new CommunityPost object to send to backend
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
                return await communityPost.Content.ReadFromJsonAsync<CommunityPost>(); // returns post object if successful
            }

            return null; // this would return null is the createpost request was not successful
        }
        #endregion

        #region Get Posts & Plans

        // this method gets all of the communityposts that are stored in the database and is called through a http get request using the below api from the controller
        // this method will return a list of communityposts
        public async Task<List<CommunityPost>> GetAllCommunityPosts()
        {
            var communityPosts = await _httpClient.GetAsync("CommunityPosts/getallposts"); // sends a http get request to the communityposts endpoint

            if (communityPosts.IsSuccessStatusCode) // checks to see whether it returns a successful status code (200)
            {
                return await communityPosts.Content.ReadFromJsonAsync<List<CommunityPost>>(); // returns the json body and deserializes the json into a communitypost object
            }

            throw new Exception("Failed to get posts, please try again!"); // failed to retrieve the posts exception message
        }

        // this method gets the workout plans that were created/added by a user (not requesting every workout plan)
        // uses the userId to retrieve a list of workout plans
        public async Task<List<WorkoutPlans>> GetWorkoutPlansByUserId(int userId)
        {
            var workoutPlans = await _httpClient.GetAsync($"WorkoutPlans/getworkoutplansbyuser/{userId}"); // sends a http get request to the api, to retrieve the list of plans

            if (workoutPlans.IsSuccessStatusCode) // checks for success
            {
                return await workoutPlans.Content.ReadFromJsonAsync<List<WorkoutPlans>>(); // shows a 200 code if success
            }

            throw new Exception("Failed to get workout plans, please try again!");
        }

        // this method gets all the meal plans created/added by a userid and retrieves them as a list
        public async Task<List<MealPlans>> GetMealPlansByUserId(int userId)
        {
            var mealPlans = await _httpClient.GetAsync($"MealPlans/getmealplansbyuser/{userId}"); // sends a http get request to the api, to retrieve the list of meal plans

            if (mealPlans.IsSuccessStatusCode) // checks for success
            {
                return await mealPlans.Content.ReadFromJsonAsync<List<MealPlans>>(); // shows a 200 code if success
            }

            throw new Exception("Failed to get workout plans, please try again!"); // exception message
        }

        // this method gets all the individual meals that an individual user has created from the database
        // returns them as a list
        public async Task<List<Meals>> GetMealsByUser(int userId)
        {
            var meals = await _httpClient.GetAsync($"MealPlans/meals/{userId}"); // sends a http get request to the api, to retrieve a list of individual meals created by a user

            if (meals.IsSuccessStatusCode) // checks for success
            {
                return await meals.Content.ReadFromJsonAsync<List<Meals>>();
            }

            throw new Exception("Failed to get meals, please try again!");
        }

        #endregion

        #region Adding External Plans and Meals

        // adds a workout plan created by another user to the logged in users account
        public async Task<bool> AddExternalUserWorkoutPlan(int planId, int userId)
        {
            var externalWorkoutPlan = await _httpClient.PostAsync($"WorkoutPlans/adduserworkoutplan/{planId}/{userId}", null); // sends a http post request with the ids

            if (externalWorkoutPlan.IsSuccessStatusCode)
            {
                return true; // success
            }

            throw new Exception("Failed to add workout plan to user, please try again!"); // exception message
        }

        // adds a meal plan created by another user to the logged in users account
        public async Task<bool> AddExternalUserMealPlans(int mealPlanId, int userId)
        {
            var externalMealPlan = await _httpClient.PostAsync($"MealPlans/addusermealplan/{mealPlanId}/{userId}", null); // sends a http post request with the ids

            if (externalMealPlan.IsSuccessStatusCode)
            {
                return true; // success
            }

            throw new Exception("Failed to add workout plan to user, please try again!"); // exception message
        }

        // adds an indiviual meal created by another user to the logged in users account
        public async Task<bool> AddExternalUserMeals(int mealId, int userId)
        {
            var externalMeals = await _httpClient.PostAsync($"MealPlans/addusermeal/{mealId}/{userId}", null); // sends a http post request with the ids

            if (externalMeals.IsSuccessStatusCode)
            {
                return true; // success
            }

            throw new Exception("Failed to add workout plan to user, please try again!"); // exception message
        }

        // this method takes all the adding external plans methods into a check, so that the there can be one universal add button on each post
        //public async Task<bool> AddExternalPlans(CommunityPost post, int userId) // instance of a community post with a userid
        //{
        //    if (post.WorkoutPlanId != null) // checks if the community post has a workout plan attached
        //    {
        //        return await AddExternalUserWorkoutPlan(post.WorkoutPlanId.Value, userId); // if it does, call this method
        //    }
        //    else if (post.MealPlanId != null) // checks if the community post has a meal plan attached
        //    {
        //        return await AddExternalUserMealPlans(post.MealPlanId.Value, userId);
        //    }
        //    else if (post.MealId != null) // checks if the community post has a meal attached
        //    {
        //        return await AddExternalUserMeals(post.MealId.Value, userId);
        //    }

        //    return false;
        //}



        public async Task<bool> AddExternalPlans(CommunityPost post, int userId)
        {
            if (post.WorkoutPlanId != null)
            {
                var response = await _httpClient.PostAsync($"WorkoutPlans/adduserworkoutplan/{post.WorkoutPlanId}/{userId}", null);
                return response.IsSuccessStatusCode;
            }
            else if (post.MealPlanId != null)
            {
                var response = await _httpClient.PostAsync($"MealPlanMeals/addusermealplan/{post.MealPlanId}/{userId}", null);
                return response.IsSuccessStatusCode;
            }
            else if (post.MealId != null)
            {
                var response = await _httpClient.PostAsync($"MealPlanMeals/addusermeal/{post.MealId}/{userId}", null);
                return response.IsSuccessStatusCode;
            }

            return false;
        }

        private async Task AddPlansToUserAccount(CommunityPost post)
        {
            try
            {
                int userId = Preferences.Get("UserId", 0);

                if (post.WorkoutPlanId != null)
                {
                    bool added = await AddExternalUserWorkoutPlan((int)post.WorkoutPlanId, userId);

                    if (added)
                        await Application.Current.MainPage.DisplayAlert("Success!", "Workout plan added to your account.", "OK");
                    else
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to add workout plan.", "OK");
                }
                else if (post.MealPlanId != null)
                {
                    bool added = await new NutritionServiceUI().AddExternalUserMealPlan((int)post.MealPlanId, userId);

                    if (added)
                        await Application.Current.MainPage.DisplayAlert("Success!", "Meal plan added to your account.", "OK");
                    else
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to add meal plan.", "OK");
                }
                else if (post.MealId != null)
                {
                    bool added = await new NutritionServiceUI().AddExternalUserMeal((int)post.MealId, userId);

                    if (added)
                        await Application.Current.MainPage.DisplayAlert("Success!", "Meal added to your account.", "OK");
                    else
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to add meal.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Unavailable", "This post doesn't contain a plan or meal.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
            }
        }

        #endregion

        #region Comments

        // creates a comment using a request body
        // using this dto reduces payload size
        public async Task<bool> CreateComment(int postId, string username, string comment)
        {
            var commentDto = new AddNewCommentDTO // a dto to match the api request
            {
                CommunityPostId = postId, // the post that the comment is being added on
                Username = username, // the user who is adding the comment
                Comment = comment // the comment
            };

            var addComment = await _httpClient.PostAsJsonAsync("CommunityPosts/addcomment", commentDto); // http post request to add comment to post

            return addComment.IsSuccessStatusCode;
        }

        // gets all the comments by the post
        public async Task<List<CommunityPostComments>> GetCommentsByPost(int postId) // returns list of comments
        {
            var comments = await _httpClient.GetAsync($"CommunityPosts/comments/{postId}"); // receives comments from a specific post

            if (comments.IsSuccessStatusCode) // checks for success
            {
                return await comments.Content.ReadFromJsonAsync<List<CommunityPostComments>>(); // retrieves list of comments
            }

            return new List<CommunityPostComments>(); // returns an empty list of comments to ensure app doesnt crash if a post hasnt been commented on
        }

        #endregion


        #region Profile

        // gets user for profile viewing
        public async Task<Users> GetUserById(int userId)
        {
            var response = await _httpClient.GetAsync($"Users/getuserbyid/{userId}"); // aoi endpoint with the inserted userId

            if (response.IsSuccessStatusCode) // checks for success
            {
                return await response.Content.ReadFromJsonAsync<Users>();
            }

            return null;
        }

        // this gets all the posts created by the user
        public async Task<List<CommunityPost>> GetCommunityPostsByUserId(int userId)
        {
            var response = await _httpClient.GetAsync($"CommunityPosts/getpostsbyuser/{userId}"); // api get request with userid input

            if (response.IsSuccessStatusCode) // checks for success
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

        #endregion


    }
}
